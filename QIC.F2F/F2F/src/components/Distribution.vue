<template>
	<div>
		<div class="search-bar">
			<Form ref="Search" :model="Search" inline>
				<Form-item prop="Contracter">
					<Input type="text" v-model="Search.Contracter" placeholder="联系人" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="Phone" style="width:100px;">
					<Input type="text" v-model="Search.Phone" placeholder="电话" autocomplete="off">
					</Input>
				</Form-item>
				<Button type="primary" icon="ios-search" @click="SearchDistribution">搜索</Button>
				<Button type="primary" icon="plus-round" @click="ShowAddDistriDetail">添加</Button>
			</Form>
		</div>
		<Table border :context="self" :columns="DistriColumn" :data="dData"></Table>
		<Page :total="count" :page-size="size" :current="currentPage" show-total show-elevator @on-change="ChangePage" style="padding:10px"></Page>
		<Modal v-model="DistriDetailModal"  @on-cancel="Cancel" :title="DistriDetailTitle" @on-ok="OkDistr" cancel-text='' width='auto' :styles="ModelStyle">
			<row>
				<Form ref="DestriDetailForm" :model="DestriDetailForm" :label-width="80" inline :rules="RuleDistr">
					<Form-item label="配送单号" prop="ID" style="width:40%;">
						<Input v-model="DestriDetailForm.ID" disabled style="width: 200px"></Input>
					</Form-item>
					<Form-item label="配送日期" prop="DistributedDate">
						<Date-picker v-model="DestriDetailForm.DistributedDate" type="datetime" @on-change="ChangeTime" placeholder="选择日期和时间" style="width: 200px"></Date-picker>
					</Form-item>
					<Form-item label="会员" prop="FamilyID" style="width:40%;">
						<Select v-model="DestriDetailForm.FamilyID" filterable :disabled="!IsAdd"  placeholder="请输入并选择"  clearable ref="FamilyID" multiple @on-change="FOnChange" @on-query-change="ChangeFamilyQuery" style="width: 200px">
							<Option v-for="item in FamilyList" :value="item.ID" :key="item" :label="item.Name"></Option>
						</Select>
					</Form-item>
					<Form-item label="手机号" prop="Mobile">
						<Input v-model="DestriDetailForm.Mobile" disabled style="width: 100px"></Input>
					</Form-item>
					<Form-item label="家庭人数" prop="FamilyNumber">
						<Input v-model="DestriDetailForm.FamilyNumber" disabled style="width: 80px"></Input>
					</Form-item>
					<Form-item label="家庭概况" prop="Description" style="width:90%">
						<Input v-model="DestriDetailForm.Description" type="textarea" disabled :autosize="{minRows: 2,maxRows: 5}"></Input>
					</Form-item>
					<Form-item label="配送信息" prop="AddressID" style="width:90%;" v-if="IsAdd">
						<Select v-model="DestriDetailForm.AddressID" clearable ref="AddressID" @on-change="ChangeAddress" :placeholder="AddressHolder" :label-in-value="true">
							<Option v-for="item in AddressList" :value="item.AddressID" :key="item">{{ item.Text }}</Option>
						</Select>
					</Form-item>
					<Form-item label="联系人" prop="Contacter" style="width:40%;">
						<Input v-model="DestriDetailForm.Contacter" style="width: 200px"></Input>
					</Form-item>
					<Form-item label="联系电话" prop="Phone" style="width:40%;">
						<Input v-model="DestriDetailForm.Phone" style="width: 200px"></Input>
					</Form-item>
					<Form-item label="送货地址" prop="Address" style="width:90%;">
						<Input v-model="DestriDetailForm.Address" style="width: 200px"></Input>
					</Form-item>
					<Form-item label="所在区域" prop="Region" style="width:90%;">
						<Input v-model="DestriDetailForm.Region" disabled></Input>
					</Form-item>
					<Form-item style="width:90%;">
						<Button type="primary" :disabled="btnSave" v-show="IsAdd" v-on:click="OkDistr()" style="padding:10px;">保存配送信息</Button>
					</Form-item>
					<Form-item label="配送清单" style="width:90%;">
						<Table :context="self" v-show="DestriListShow" border :columns="DistriListColumn" :data="ListData"></Table>
						<Button type="dashed" long icon="plus-round" @click="ShowAddList">添加新清单</Button>
					</Form-item>
				</Form>
			</row>
		</Modal>
		<Modal v-model="modalList"  :title="ListTitle" cancel-text='' width='360' @on-ok="OkList">
			<Form ref="ListForm" :model="ListForm" :label-width="80" :rules="ListRule">				
				<Form-item label="家庭卡" prop="FamilyCardID">
					<Select v-model="ListForm.FamilyCardID" ref="FamilyCardID" style="width: 200px">
						<Option v-for="item in FamilyCardData" :value="item.ID+''" :key="item" :label="item.CardKindName"></Option>
					</Select>
				</Form-item>
				<Form-item label="产品" prop="ProductID">
					<Select v-model="ListForm.ProductID" clearable ref="ProductID" :filterable="filterable" style="width: 200px">
						<Option v-for="item in ProductData" :value="item.ID+''" :key="item" :label="item.Name"></Option>
					</Select>
				</Form-item>
				<Form-item label="数量" prop="Amount">
					<Input-number v-model="ListForm.Amount" :min="1" placeholder="请输入" style="width: 200px"></Input-number>
				</Form-item>
				<Form-item label="溯源报告" prop="ReportID">
					<Select v-model="ListForm.ReportID" clearable ref="ReportID" :filterable="filterable" style="width: 200px">
						<Option v-for="item in ReportData" :value="item.ID+''" :key="item" :label="item.Name"></Option>
					</Select>
				</Form-item>
			</Form>
		</Modal>
	</div>
</template>
<script>
	import testData from '@/json/TestData.json' //测试用
	export default {
		data() {
				return {
					Search: {
						Contracter: '',
						Phone: ''
					},
					filterable: true,
					AddressHolder: '没有可用地址',
					DestriListShow: true,
					self: this,
					btnSave: false,
					count: 0,
					size: 10,
					currentPage: 1,
					IsAdd: true,
					FamilyCardData: [],
					ProductData: [],
					ReportData: [],
					DestriDetailForm: {
						ID: '',
						Mobile: '',
						DistributedDate: '',
						FamilyID: [],
						Description: '',
						FamilyNumber: 0,
						Contacter: '',
						Phone: '',
						Address: '',
						Region: '',
						AddressID: 0,
						DistriList: []
					},
					RuleDistr: {
						FamilyID: [{
							required: true,
							type: 'array',
							message: '请选择会员',
							min: 1,
							trigger: 'blur'
						}],
						DistributedDate: [{
							required: true,
							message: '请选择时间',
							trigger: 'blur'
						}],
						Address: [{
							required: true,
							message: '请输入地址',
							trigger: 'blur'
						}],
						Phone: [{
							required: true,
							message: '请输入联系电话',
							trigger: 'blur'
						}],
						Contacter: [{
							required: true,
							message: '请输入联系人',
							trigger: 'blur'
						}]
					},
					ListForm: {
						ID: 0,
						FamilyCardID: '',
						ProductID: '',
						Amount: 1,
						ReportID: ''
					},
					ListRule: {
						FamilyCardID: [{
							required: true,
							message: '请选择家庭卡',
							trigger: 'blur'
						}],
						ProductID: [{
							required: true,
							message: '请选择产品',
							trigger: 'blur'
						}],
						ReportID: [{
							required: true,
							message: '请选择报告',
							trigger: 'blur'
						}],
					},
					ListIsAdd: true,
					modalList: false,
					ListTitle: ' ',
					AddressList: [],
					DistriDetailModal: false,
					ModelStyle: {
						padding: '20px',
						maxWidth: '800px'
					},

					DistriColumn: [{
						title: '配送编号',
						align: 'center',
						key: 'ID'
					}, {
						title: '联系人',
						align: 'center',
						key: 'Contacter'
					}, {
						title: '联系电话',
						align: 'center',
						key: 'Phone'
					}, {
						title: '配送地址',
						align: 'center',
						key: 'Address'
					}, {
						title: '配送日期',
						align: 'center',
						key: 'DistributedDate'
					}, {
						title: '创建日期',
						align: 'center',
						key: 'CreateTime'
					}, {
						title: '操作',
						key: 'Operate',
						width: 100,
						align: 'center',
						render(row, column, index) {
							return `<i-button type="primary" size="small" @click="ShowDistriDetial(${index})">查看编辑</i-button> `;
						}
					}], //
					DistriDetailTitle: ' ',
					DistriListColumn: [{
						title: '家庭卡',
						align: 'center',
						key: 'FamilyCardName'
					}, {
						title: '产品',
						align: 'center',
						key: 'ProductName'
					}, {
						title: '数量',
						align: 'center',
						key: 'Amount'
					}, {
						title: '溯源报告',
						align: 'center',
						key: 'ReportName'
					}, {
						title: '操作',
						key: 'Operater',
						width: 120,
						align: 'center',
						render(row, column, index) {
							return `<i-button type="primary" size="small" @click="ShowUpdateList(${index})"> 编辑 </i-button> <i-button type="primary" size="small" @click="DeleteList(${index})"> 删除 </i-button> `;
						}
					}],
					ListData: [],
					dData: [],
					FamilyList: [{
						ID: 0,
						Name: ' '
					}],
					FamilyIdCurrentCount: 0,
				}
			},
			watch: {
				'ListData': 'watchListData',
				"modalList": "watchModalList"
			},
			methods: {
				Cancel() {
					this.FamilyList = [];
				},
				watchModalList() {
					if(this.modalList) {
						this.ajax({
							url: this.$store.state.urlConfigs.GetCardKindByFamily,
							params: {
								familyID: this.DestriDetailForm.FamilyID[0]
							},
							type: 'get',
							success(data) {
								this.FamilyCardData = data.data.Result;
							},
							error() {}
						});
					}
				},
				watchListData() {
					if(this.ListData.length > 0)
						this.DestriListShow = true;
					else this.DestriListShow = false;
				}, //监听表格数据事件			
				OkDistr() {
					this.$refs['DestriDetailForm'].validate((valid) => {
						var _this = this;
						if(valid) {
							_this.$Loading.start();
							_this.btnSave = true;
							if(this.IsAdd) {
								_this.ajax({
									type: 'post',
									params: {
										ID: _this.DestriDetailForm.ID,
										FamilyID: _this.DestriDetailForm.FamilyID[0],
										DistributedDate: _this.DestriDetailForm.DistributedDate,
										Address: _this.DestriDetailForm.Address,
										Phone: _this.DestriDetailForm.Phone,
										Contacter: _this.DestriDetailForm.Contacter
									},
									url: _this.$store.state.urlConfigs.AddDistribution,
									success: function(data) {
										if(data.data.DistributionError === 1) {
											this.IsAdd = false;
											this.DistriDetailTitle = '编辑配送订单'
											this.$Message.success('保存订单信息成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchDistribution();
										_this.btnSave = false;

									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('添加失败!');
										_this.btnSave = false;
									}
								});

							} else {
								_this.btnSave = false;
								_this.ajax({
									type: 'post',
									params: {
										ID: _this.DestriDetailForm.ID,
										FamilyID: _this.DestriDetailForm.FamilyID[0],
										DistributedDate: _this.DestriDetailForm.DistributedDate,
										Address: _this.DestriDetailForm.Address,
										Phone: _this.DestriDetailForm.Phone,
										Contacter: _this.DestriDetailForm.Contacter
									},
									url: _this.$store.state.urlConfigs.UpdateDistribution,
									success: function(data) {
										if(data.data) {
											this.$Message.success('修改成功!');
										} else {
											_this.$Message.error('修改失败');
										}
										_this.$Loading.finish();
										_this.SearchDistribution();
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('修改失败!');
									}
								});
							}
						} else {
							this.$Message.error('表单验证失败!');
							//							this.$refs['FamilyForm'].resetFields();
						}
					})
				},
				OkList() {
					this.$refs['ListForm'].validate((valid) => {
						var _this = this;
						if(valid) {
							_this.$Loading.start();
							if(this.ListIsAdd) {
								_this.ajax({
									type: 'post',
									params: {
										DistributionID:this.DestriDetailForm.ID,
										FamilyID: _this.DestriDetailForm.FamilyID[0],
										FamilyCardID:this.ListForm.FamilyCardID,
										ProductID:this.ListForm.ProductID,
										Amount:this.ListForm.Amount,
										ReportID:this.ListForm.ReportID
									},
									url: _this.$store.state.urlConfigs.AddDistributionDetail,
									success: function(data) {
										if(data.data.DistributionDetailError===1) {
											this.$Message.success('添加配送信息成功!');											
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchListData(_this.DestriDetailForm.ID);
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('添加失败!');
										
									}
								});

							} else {								
								_this.ajax({
									type: 'post',
									params: {
										ID: _this.ListForm.ID,
										DistributionID:this.DestriDetailForm.ID,
										FamilyID: _this.DestriDetailForm.FamilyID[0],
										FamilyCardID:this.ListForm.FamilyCardID,
										ProductID:this.ListForm.ProductID,
										Amount:this.ListForm.Amount,
										ReportID:this.ListForm.ReportID
									},
									url: _this.$store.state.urlConfigs.UpdateDistributionDetail,
									success: function(data) {
										if(data.data.DistributionDetailError===1) {
											this.$Message.success('修改成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchListData(_this.DestriDetailForm.ID);
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('修改失败!');
									}
								});
							}
						} else {
							this.$Message.error('表单验证失败!');
							//							this.$refs['FamilyForm'].resetFields();
						}
					})
				},
				ShowAddList() {
					if(this.IsAdd) {
						this.$Message.warning('请先保存订单信息!');
						return;
					}
					var _this = this;
					this.$refs["ListForm"].resetFields();
					this.ListTitle = '添加配送清单';
					this.modalList = true;
					this.ListIsAdd = true;
					setTimeout(function() {
						_this.$refs["ProductID"].clearSingleSelect();
						_this.$refs["ReportID"].clearSingleSelect();
					}, 600)
				},
				ShowUpdateList(index) {
					this.modalList = true;
					var _this = this;
					this.ListTitle = '编辑配送清单';
					var row = this.ListData[index];
					this.ListIsAdd = false;
					setTimeout(function() {
						_this.$set(_this.ListForm, 'ID', row.ID);
						_this.$set(_this.ListForm, 'FamilyCardID', row.FamilyCardID+'');
						_this.$set(_this.ListForm, 'ProductID', row.ProductID+'');
						_this.$set(_this.ListForm, 'Amount', row.Amount);
						_this.$set(_this.ListForm, 'ReportID', row.ReportID+'');
						_this.$refs['ListForm'].validate((valid) => {});
					}, 500)
					

				},
				DeleteList(index) {
					var row = this.ListData[index];
					var _this = this;
					this.$Modal.confirm({
						title: '提示',
						content: '确定删除这条数据?',
						onCancel() {
							return;
						},
						onOk() {
							_this.ajax({
								url: _this.$store.state.urlConfigs.DeleteDistributionDetailByID,
								params: {
									id: row.ID
								},
								type: 'get',
								success(data) {
									if(data.data) {
										_this.$Message.success('删除成功!');
										_this.SearchListData(_this.DestriDetailForm.ID);
									} else {
										_this.$Message.error('删除失败');
									}
								},
								error() {
									_this.$Message.error('删除失败');
								}
							});
						}
					})
				},
				ChangeAddress(value) {
					var label = value.label;
					var infos = label.split(' - ');
					if(infos.length === 0) return;
					this.$set(this.DestriDetailForm, 'Phone', infos[1]);
					this.$set(this.DestriDetailForm, 'Address', value.value);
					this.$set(this.DestriDetailForm, 'Contacter', infos[0]);
					this.$set(this.DestriDetailForm, 'Address', infos[2]);
				},
				ShowDistriDetial(index) {
					var row = this.dData[index];
					this.DistriDetailModal = true;
					this.IsAdd = false;
					this.DistriDetailTitle = "编辑配送订单";
					this.FamilyList = [{
						ID: row.FamilyID,
						Name: row.FamilyName
					}]
					this.$set(this.DestriDetailForm, 'FamilyID', [row.FamilyID]);
					this.$set(this.DestriDetailForm, 'ID', row.ID);
					this.$set(this.DestriDetailForm, 'Phone', row.Phone);
					this.$set(this.DestriDetailForm, 'Contacter', row.Contacter);
					this.$set(this.DestriDetailForm, 'DistributedDate', row.DistributedDate);
					this.$set(this.DestriDetailForm, 'Address', row.Address);
					this.$set(this.DestriDetailForm, 'Description', row.Description);
					this.$set(this.DestriDetailForm, 'FamilyNumber', row.FamilyNumber);
					this.$refs['DestriDetailForm'].validate((valid) => {});
					this.SearchListData(row.ID);
				},
				ShowAddDistriDetail() {
					this.$refs['DestriDetailForm'].resetFields();
					this.DistriDetailTitle = "添加配送订单";
					this.ajax({
						url: this.$store.state.urlConfigs.GetDistributionID,
						type: 'get',
						success(data) {
							this.$set(this.DestriDetailForm, 'ID', data.data);
						},
						error() {
							this.$Message.error('获取订单号失败!');
						}
					});
					this.$set(this.DestriDetailForm, 'Phone', '');
					this.$set(this.DestriDetailForm, 'FamilyID', []);
					this.$set(this.DestriDetailForm, 'Contacter', '');
					this.$set(this.DestriDetailForm, 'DistributedDate', '');
					this.$set(this.DestriDetailForm, 'Address', '');
					this.$set(this.DestriDetailForm, 'Description', '');
					this.$set(this.DestriDetailForm, 'FamilyNumber', '');
					this.$set(this.DestriDetailForm, 'Region', '');
					this.$set(this.DestriDetailForm, 'AddressID', 0);
					this.$set(this.DestriDetailForm, 'Mobile', '');
					this.AddressList = [];
					this.ListData = [];
					this.IsAdd = true;
					this.DistriDetailModal = true;
				},
				SearchDistribution(changePage) {
					var _this = this;
					if(changePage !== true) {
						_this.$nextTick(function() {
							_this.currentPage = 1;
						});
						_this.currentPage = 1;
					}
					_this.Search.PageIndex = _this.currentPage;
					_this.Search.PageSize = _this.size;
					_this.$Loading.start();
					_this.ajax({
						url: _this.$store.state.urlConfigs.GetDistributionList,
						type: 'post',
						params: _this.Search,
						success: function(data) {
							_this.dData = data.data.Result;
							_this.count = data.data.TotalCount
							_this.$Loading.finish();
						},
						error: function() {
							_this.$Loading.error();
							_this.$Message.error('查询失败!');
						}
					});
				},
				SearchListData(id) {
					this.ajax({
						url: this.$store.state.urlConfigs.GetDetailByDistributionID,
						params: {
							distributionID: id
						},
						type: 'get',
						success(data) {
							this.ListData = data.data;
						},
						error() {
							_this.$Message.error('获取配送清单失败!');
						}
					});
				},
				ChangePage(p) {
					this.currentPage = p;
					this.SearchDistribution(true);
				},
				ChangeTime(time) {					
					this.DestriDetailForm.DistributedDate = time;
				},
				ChangeFamilyQuery(q) {
					this.ajax({
						url: this.$store.state.urlConfigs.GetFamilyInfoListByNameFuzzy,
						type: 'get',
						success(data) {
							if(data.data.length === 0 && this.FamilyIDCurrentCount == 1) {
								this.FamilyIDCurrentCount = 0;
								return;
							}
							if(data.data.length === 0) {
								this.$set(this.DestriDetailForm, 'Mobile', '');
								this.$set(this.DestriDetailForm, 'Description', '');
								this.$set(this.DestriDetailForm, 'FamilyNumber', 0);
								this.$set(this.DestriDetailForm, 'Region', '');
								this.$set(this.DestriDetailForm, 'Phone', '');
								//								this.$set(this.DestriDetailForm, 'Contacter', '');
								//								this.$set(this.DestriDetailForm, 'AddressID', 0);
								//								this.$set(this.DestriDetailForm, 'Address', '');
								//								this.AddressList = [];
								//								this.$refs["AddressID"].clearSingleSelect();
							}
							this.$nextTick(function() {
								var list = [];
								for(var i = 0; i < data.data.length; i++) {
									list.push({
										ID: data.data[i].ID,
										Name: data.data[i].Name
									});
								}
								this.FamilyList = list;
							});

						},
						params: {
							name: q,
							count: 10
						}
					});

				},
				FOnChange(a) {
					if(a.length > 0) {
						this.FamilyIDCurrentCount = 1;
						this.ajax({
							url: this.$store.state.urlConfigs.GetFamilyInfoByID,
							params: {
								id: a[0]
							},
							success(data) {
								this.$set(this.DestriDetailForm, 'Mobile', data.data.Mobile);
								this.$set(this.DestriDetailForm, 'Description', data.data.Description);
								this.$set(this.DestriDetailForm, 'FamilyNumber', data.data.FamilyNumber);
								this.$set(this.DestriDetailForm, 'Region', data.data.Region);
								this.ajax({
									url: this.$store.state.urlConfigs.GetFamilyAddressByFamilyID,
									params: {
										familyid: a[0]
									},
									success(d) {
										var list = new Array();
										var result = d.data.Result;
										if(result.length === 0 && this.IsAdd) {
											this.AddressList = [];
											this.$set(this.DestriDetailForm, 'Phone', '');
											this.$set(this.DestriDetailForm, 'Contacter', '');
											this.$set(this.DestriDetailForm, 'AddressID', 0);
											this.$set(this.DestriDetailForm, 'Address', '');
											this.AddressHolder = '没有可用地址';
											return;
										}
										for(var i = 0; i < result.length; i++) {
											list.push({
												AddressID: result[i].ID,
												Text: `${result[i].Contacter} - ${result[i].Phone} - ${result[i].Address}`
											});

										}
										this.AddressList = list;
										this.AddressHolder = '请选择地址'
									}
								})
							}
						});
					}
					if(this.DestriDetailForm.FamilyID.length > 1) {
						this.DestriDetailForm.FamilyID.reverse();
						this.DestriDetailForm.FamilyID.pop();
					}
					if(this.DestriDetailForm.FamilyID[0] === 0) {
						this.DestriDetailForm.FamilyID.pop();
					}
				},
				FCOnChange(a) {

				}
			},
			created() {
				this.SearchDistribution();
				this.ajax({
					url: this.$store.state.urlConfigs.GetAllProduct,
					type: 'get',
					success(data) {
						this.ProductData = data.data;
					},
					error() {
						this.$Message.error('获取产品数据失败!');
					}
				});
				this.ajax({
					url: this.$store.state.urlConfigs.GetAllProductReport,
					type: 'get',
					success(data) {
						this.ReportData = data.data;
					},
					error() {
						_this.$Message.error('获取报告数据失败!');
					}
				});
			}
	}
</script>
<style>
	.col-border {
		border-right: thin solid #B2B2B2;
		height: 40px;
		line-height: 40px;
	}
</style>