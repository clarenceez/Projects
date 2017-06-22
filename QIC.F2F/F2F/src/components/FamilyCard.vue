<template>
	<div>
		<div class="search-bar">
			<Form ref="Search" :model="Search" inline>
				<Form-item prop="FamilyName">
					<Input type="text" v-model="Search.FamilyName" placeholder="家庭" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="CardKindName">
					<Input type="text" v-model="Search.CardKindName" placeholder="卡类型" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="StartTime">
					<Date-picker type="date" v-model="Search.StartTime" @on-change="ChangeStartTime" :editable="false" placeholder="选择日期" style="width: 160px"></Date-picker>
				</Form-item>
				<Form-item prop="EndTime">
					<Date-picker type="date" v-model="Search.EndTime" @on-change="ChangeEndTime" :editable="false" placeholder="选择日期" style="width: 160px"></Date-picker>
				</Form-item>
				<Button type="primary" icon="ios-search" @click="SearchFamilyCard()">搜索</Button>
				<Button type="primary" icon="plus-round" @click="ShowAddFamilyCard">添加</Button>
			</Form>

		</div>
		<Table border :context="self" :columns="columns" :data="FamilyCardData"></Table>
		<Page :total="count" :page-size="size" :current="currentPage" show-total show-elevator @on-change="ChangePage" style="padding:10px"></Page>
		<Modal v-model="FamilyCardModal"  @on-cancel="Cancel" @on-ok="OkFamilyCard" :title="FamilyCardTitle" cancel-text='' width='auto' :styles="FamilyCardStyle">
			<Form ref="FamilyCardForm" :model="FamilyCardForm" :label-width="120" :rules="Rule">
				<Form-item label="家庭" prop="FamilyID">
					<Select v-model="FamilyCardForm.FamilyID" placeholder="请输入并选择" filterable clearable ref="FamilyID" multiple @on-change="FOnChange" @on-query-change="ChangeFamilyQuery">
						<Option v-for="item in FamilyList" :value="item.ID" :key="item" :label="item.Name"></Option>
					</Select>
				</Form-item>
				<Form-item label="卡类型" prop="CardKindID">
					<Select v-model="FamilyCardForm.CardKindID" placeholder="请输入并选择" filterable clearable ref="CardKindID" multiple @on-change="COnChange" @on-query-change="ChangeCardQuery">
						<Option v-for="item in CardKindIDList" :value="item.ID" :key="item" :label="item.Name"></Option>
					</Select>
				</Form-item>
				<Form-item label="可配送次数" prop="TotalCount" v-show="!IsAdd">
					<Input-number v-model="FamilyCardForm.TotalCount" :min="0" placeholder="请输入"></Input-number>
				</Form-item>
				<Form-item label="已配送次数" prop="DistributedCount" v-show="!IsAdd">
					<Input-number v-model="FamilyCardForm.DistributedCount" :min="0" placeholder="请输入..."></Input-number>
				</Form-item>
				<Form-item label="是否有效" prop="IsValid">
					<Select v-model="FamilyCardForm.IsValid" placeholder="请选择">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
			</Form>

		</Modal>
	</div>
</template>
<script>	
	export default {
		data() {
				return {
					FamilyList: [{
						ID: 0,
						Name: ' '
					}],
					CardKindIDList: [{
						ID: 0,
						Name: ' '
					}],
					FamilyIdCurrentCount: 0,
					CardKindIdCurrentCount: 0,
					Search: {
						FamilyName: '',
						CardKindName: '',
						StartTime: '',
						EndTime: ''
					},
					self: this,
					count: 0,
					size: 10,
					currentPage: 1,
					FamilyCardTitle: ' ',
					FamilyCardModal: false,
					FamilyCardStyle: {
						padding: '20px',
						maxWidth: '600px'
					},
					IsAdd: true,
					FamilyCardForm: {
						ID: 0,
						FamilyName: '',
						FamilyID: [0],
						CardKindName: '',
						CardKindID: [0],
						TotalCount: 0,
						IsValid: 'true',
						DistributedCount: 0,
						CreateTime: ''
					},
					Rule: {
						FamilyID: [{
							required: true,
							message: '请选择家庭',
							min: 1,
							type: 'array',
							trigger: 'blur'
						}],
						CardKindID: [{
							required: true,
							type: 'array',
							min: 1,
							message: '请选择卡类型',
							trigger: 'blur'
						}]
					}, //信息验证规则
					FamilyCardData: [],
					columns: [{
						title: '家庭',
						key: 'FamilyName',
						align: 'center'
					}, {
						title: '卡类型',
						align: 'center',
						key: 'CardKindName'
					}, {
						title: '可配送次数',
						key: 'TotalCount',
						align: 'center'
					}, {
						title: '已配送次数',
						key: 'DistributedCount',
						align: 'center'
					}, {
						title: '是否有效',
						key: 'IsValid',
						align: 'center',
						render(row, column, index) {
							if((row.IsValid + '') == 'true') return '是';
							return '否';
						}
					}, {
						title: '创建时间',
						key: 'CreateTime',
						align: 'center'
					}, {
						title: '操作',
						key: 'Operate',
						align: 'center',
						render(row, column, index) {
							return `<i-button type='primary' @click="ShowUpdateFamilyCard(${index})"> 编辑 </i-button>`;
						}
					}]
				}
			},
			methods: {
				Cancel() {
					this.FamilyList = [];
					this.CardKindIDList = [];
				},
				OkFamilyCard() {
					this.$refs['FamilyCardForm'].validate((valid) => {
						var _this = this;
						if(valid) {
							_this.$Loading.start();
							if(_this.IsAdd) {
								_this.ajax({
									type: 'post',
									params: {
										FamilyID: this.FamilyCardForm.FamilyID[0],
										CardKindID: this.FamilyCardForm.CardKindID[0],
										TotalCount: this.FamilyCardForm.TotalCount,
										DistributedCount: this.FamilyCardForm.DistributedCount,
										IsValid: this.FamilyCardForm.IsValid
									},
									url: _this.$store.state.urlConfigs.AddFamilyCard,
									success: function(data) {
										if(data.data.FamilyCardError === 1) {
											_this.$Message.success('添加成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchFamilyCard();
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('修改失败!');
									}
								});
							} else {
								_this.ajax({
									type: 'post',
									params: {
										ID: this.FamilyCardForm.ID,
										FamilyID: this.FamilyCardForm.FamilyID[0],
										CardKindID: this.FamilyCardForm.CardKindID[0],
										TotalCount: this.FamilyCardForm.TotalCount,
										DistributedCount: this.FamilyCardForm.DistributedCount,
										IsValid: this.FamilyCardForm.IsValid
									},
									url: _this.$store.state.urlConfigs.UpdateFamilyCardKind,
									success: function(data) {
										if(data.data.FamilyCardError === 1) {
											_this.$Message.success('修改成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchFamilyCard();
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('修改失败!');
									}
								});
							}
						} else {
							this.$Message.error('表单验证失败!');
						}
					});
					this.FamilyList = [];
					this.CardKindIDList = [];
				},
				ShowUpdateFamilyCard(index) {
					var row = this.FamilyCardData[index];
					this.FamilyList = [{
						ID: row.FamilyID,
						Name: row.FamilyName
					}];
					this.CardKindIDList = [{
						ID: row.CardKindID,
						Name: row.CardKindName
					}]
					this.FamilyCardTitle = '编辑家庭卡类型';
					this.FamilyCardModal = true;
					this.IsAdd = false;
					this.$set(this.FamilyCardForm, 'ID', row.ID);
					this.$set(this.FamilyCardForm, 'FamilyID', [row.FamilyID]);
					this.$set(this.FamilyCardForm, 'CardKindID', [row.CardKindID]);
					this.$set(this.FamilyCardForm, 'TotalCount', row.TotalCount);
					this.$set(this.FamilyCardForm, 'DistributedCount', row.DistributedCount);
					this.$set(this.FamilyCardForm, 'IsValid', row.IsValid + '');
					//					this.$set(this.FamilyCardForm, 'CreateTime', row.CreatimeTime);
				},
				ShowAddFamilyCard() {
					this.FamilyCardTitle = '添加家庭卡类型';
					this.FamilyCardModal = true;
					this.$refs['FamilyCardForm'].resetFields();
					this.$set(this.FamilyCardForm, 'FamilyID', []);
					this.$set(this.FamilyCardForm, 'CardKindID', []);
					this.IsAdd = true;
				},
				SearchFamilyCard(changePage) {
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
						url: _this.$store.state.urlConfigs.GetFamilyCardList,
						type: 'post',
						params: _this.Search,
						success: function(data) {
							_this.FamilyCardData = data.data.Result;
							_this.count = data.data.TotalCount
							_this.$Loading.finish();
						},
						error: function() {
							_this.$Loading.error();
							_this.$Message.error('查询失败!');
						}
					});
				},
				ChangePage(p) {
					this.currrentPage = p;
					this.SearchFamilyCard(true);
				},
				ChangeStartTime(time) {
					this.Search.StartTime = time;
				},
				ChangeEndTime(time) {
					this.Search.EndTime = time;
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
				ChangeCardQuery(q) {
					this.ajax({
						url: this.$store.state.urlConfigs.GetCardKindListByNameFuzzy,
						type: 'get',
						success(data) {
							if(data.data.length === 0 && this.CardKindIdCurrentCount == 1) {
								this.CardKindIdCurrentCount = 0;
								return;
							}

							this.$nextTick(function() {
								var list = [];
								for(var i = 0; i < data.data.length; i++) {
									list.push({
										ID: data.data[i].ID,
										Name: data.data[i].Name
									});
								}
								this.CardKindIDList = list;
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
					}
					if(this.FamilyCardForm.FamilyID.length > 1) {
						this.FamilyCardForm.FamilyID.reverse();
						this.FamilyCardForm.FamilyID.pop();
					}
					if(this.FamilyCardForm.FamilyID[0] === 0) {
						this.FamilyCardForm.FamilyID.pop();
					}
				},
				COnChange(a) {
					if(a.length > 0) {
						this.CardKindIdCurrentCount = 1;
					}
					if(this.FamilyCardForm.CardKindID.length > 1) {						
						this.FamilyCardForm.CardKindID.reverse();
						this.FamilyCardForm.CardKindID.pop();
					}
					if(this.FamilyCardForm.CardKindID[0] === 0) {
						this.FamilyCardForm.CardKindID.pop();
					}
				}

			},
			created() {				
				this.SearchFamilyCard();
			}
	}
</script>