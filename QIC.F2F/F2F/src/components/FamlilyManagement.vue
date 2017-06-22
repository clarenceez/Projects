<template>
	<div>
		<div class="search-bar">
			<Form ref="Search" :model="Search" inline>
				<Form-item prop="Name">
					<Input type="text" v-model="Search.Name" placeholder="会员名" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="Mobile">
					<Input v-model="Search.Mobile" placeholder="电话" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="IsClosed" style="width:100px;">
					<Select v-model="Search.IsClosed" clearable placeholder="是否关闭">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Button type="primary" icon="ios-search" @click="searchFamily">搜索</Button>
				<Button type="primary" icon="plus-round" @click="AddFamily">添加</Button>
			</Form>
		</div>
		<Table border :context="self" :columns="columns" :data="fdata"></Table>
		<Page :total="count" :page-size="size" :current="currentPage" show-total show-elevator @on-change="ChangePage" style="padding:10px"></Page>
		<Modal v-model="modal1" :title="address" @on-ok="ok" cancel-text='' width='auto' :styles="addressStyle">
			<Button type="primary" icon="plus-round" v-on:click="AddNewAddress" style="padding:10px;">添加新地址</Button>
			<Table border :context="self" :columns="addressCol" :data="addressData" style="margin-top:10px"></Table>
		</Modal>
		<Modal v-model="modalFamily" :title="FamilyTitle" @on-ok="OkFamily" cancel-text='' width='800'>
			<Form ref="FamilyForm" :model="FamilyForm" :label-width="80" :rules="FamilyRule">
				<Form-item label="姓名" prop="Name">
					<Input v-model="FamilyForm.Name" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="电话" prop="Mobile">
					<Input v-model="FamilyForm.Mobile" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="区域" prop="Region">
					<Input v-model="FamilyForm.Region" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="人数" prop="FamilyNumber">
					<Input-number v-model="FamilyForm.FamilyNumber" :min="1" placeholder="请输入"></Input-number>
				</Form-item>
				<Form-item label="家庭概况" prop="Description">
					<Input v-model="FamilyForm.Description" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="是否关闭">
					<Select v-model="FamilyForm.IsClosed" placeholder="请选择">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Form-item>
					<Button type="primary" :disabled="btnSave" v-show="CurrentFamilyID==0" v-on:click="OkFamily()" style="padding:10px;">保存家庭信息</Button>
				</Form-item>
				<Form-item label="配送地址" prop="FamilyAddress">
					<Table border v-show="FamilyAddressShow" :context="self" :columns="addressCol" :data="addressData" style="margin-top:10px"></Table>
					<Button type="dashed" long v-on:click="AddNewAddress" icon="plus-round">添加新地址</Button>
				</Form-item>
			</Form>

		</Modal>
		<Modal v-model="modal2" :title="AddressTitle" @on-ok="OkAddress()" @on-cancel="CancelAddress()" width='400' :styles="addressStyle">
			<Form ref="AddressForm" :model="AddressForm" :label-width="80" :rules="AddressRule">
				<Form-item label="联系人" prop="Contacter">
					<Input v-model="AddressForm.Contacter" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="联系电话" prop="Phone">
					<Input v-model="AddressForm.Phone" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="邮编" prop="PostCode">
					<Input v-model="AddressForm.PostCode" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="是否关闭">
					<Select v-model="AddressForm.IsClosed" placeholder="请选择">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Form-item label="地址" prop="Address">
					<Input v-model="AddressForm.Address" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入..."></Input>
				</Form-item>
			</Form>
		</Modal>
	</div>
</template>

<script>
	import httpHelper from '@/js/httpHelp'
	import '@/css/site.css'
	export default {
		data() {
				return {
					Search: {
						Name: '',
						Mobile: '',
						IsClosed: ''
					}, //搜索表单
					address: ' ', //显示收货地址的题目
					modal1: false, //是否显示收货地址表格弹出层
					modal2: false, //是否显示修改或者添加收货地址弹出层
					modalFamily: false, //是否显示家庭信息弹出层
					count: 0, //页码总数
					size: 10, //每页行数
					currentPage: 1,
					FamilyIsAdd: true, //是否是添加家庭信息
					FamilyCurrentRow: 0, //当前选择家庭信息行数,
					FamilyTitle: ' ',
					FamilyAddressShow: true, //是否显示
					CurrentFamilyID: 0,
					self: this,
					btnSave: false,
					columns: [{
						title: '会员',
						align: 'center',
						key: 'Name',
						render(row, column, index) {
							return `<a @click="UpdateFamily(${index})">${row.Name}</a> `;
						}
					}, {
						title: '昵称',
						align: 'center',
						key: 'NickName'
					}, {
						title: '电话',
						align: 'center',
						key: 'Mobile'
					}, {
						title: '区域',
						align: 'center',
						key: 'Region'
					}, {
						title: '人数',
						align: 'center',
						key: 'FamilyNumber'
					}, {
						title: '是否关闭',
						align: 'center',
						key: 'IsClosed',
						render(row, column, index) {
							if((row.IsClosed + '') == 'true') return '是';
							return '否';
						}
					}, {
						title: '配送地址',
						key: 'FamilyAddress',
						width: 100,
						align: 'center',
						render(row, column, index) {
							return `<i-button type="primary" size="small" @click="show(${index})">查看地址</i-button> `;
						}
					}], //家庭信息
					fdata: [], //家庭信息数据
					addressCol: [{
						title: '联系人',
						align: 'center',
						key: 'Contacter'
					}, {
						title: '联系电话',
						align: 'center',
						key: 'Phone'
					}, {
						title: '地址',
						align: 'center',
						key: 'Address'
					}, {
						title: '邮编',
						align: 'center',
						key: 'PostCode'
					}, {
						title: '是否关闭',
						align: 'center',
						key: 'IsClosed',
						render(row, column, index) {
							if((row.IsClosed + '') == 'true') return '是';
							return '否';
						}
					}, {
						title: '操作',
						key: 'Operate',
						width: 100,
						align: 'center',
						render(row, column, index) {
							return `<i-button type="primary" size="small" @click="UpdateAddress(${index})">修改</i-button> `;

						}
					}], //地址信息,
					AddressTitle: ' ', //地址标题
					addressData: [], //地址信息数据
					addressStyle: {
						padding: '20px',
						maxWidth: '1200px'
					}, //地址信息表格样式
					AddressForm: {
						ID: 0,
						Contacter: '',
						IsClosed: 'false',
						Phone: '',
						Address: '',
						PostCode: ''
					}, //地址信息表单
					AddressIsAdd: true, //是否是添加地址信息
					AddressCurrentRow: 0, //当前选择地址信息行数
					AddressRule: {
						Contacter: [{
							required: true,
							message: '请填写联系人',
							trigger: 'blur'
						}],
						Phone: [{
							required: true,
							message: '请填写手机号',
							trigger: 'blur'
						}, {
							type: 'string',
							min: 11,
							max: 11,
							message: '请输入正确的手机号',
							trigger: 'blur'
						}],
						PostCode: [{
							required: true,
							message: '请填写邮编',
							trigger: 'blur'
						}],
						Address: [{
							required: true,
							message: '请填写地址',
							trigger: 'blur'
						}]
					}, //地址信息验证规则
					FamilyForm: {
						ID: 0,
						Name: '',
						Description: '',
						Mobile: '',
						Region: '',
						FamilyNumber: 1,
						IsClosed: 'false',
						FamilyAddress: []
					}, //家庭信息表单
					FamilyRule: {
						Name: [{
							required: true,
							message: '请填写姓名',
							trigger: 'blur'
						}],
						Mobile: [{
							required: true,
							message: '请填写手机号',
							trigger: 'blur'
						}, {
							type: 'string',
							min: 11,
							max: 11,
							message: '请输入正确的手机号',
							trigger: 'blur'
						}],
						Description: [{
							required: true,
							message: '请填写家庭概况',
							trigger: 'blur'
						}],
						Region: [{
							required: true,
							message: '请填写地区',
							trigger: 'blur'
						}]
					} //家庭信息验证规则
				}
			},
			watch: {
				'addressData': 'watchAddressData'
			},
			methods: {
				watchAddressData() {
					if(this.addressData.length > 0)
						this.FamilyAddressShow = true;
					else this.FamilyAddressShow = false;
				}, //监听地址表格数据事件
				ok() {

				},
				CancelAddress() { //地址弹出层取消、关闭事件回调
					this.$set(this.AddressForm, 'Contacter', '');
					this.$set(this.AddressForm, 'IsClosed', 'false');
					this.$set(this.AddressForm, 'Phone', '');
					this.$set(this.AddressForm, 'Address', '');
					this.$set(this.AddressForm, 'PostCode', '');
				},
				OkAddress() { 			
					this.$refs['AddressForm'].validate((valid) => {
						var _this = this;
						if(valid) {
							_this.$Loading.start();
							if(this.AddressIsAdd) {
								_this.ajax({
									type: 'post',
									params: {
										FamilyID: this.CurrentFamilyID,
										Contacter: this.AddressForm.Contacter,
										Phone: this.AddressForm.Phone,
										Address: this.AddressForm.Address,
										PostCode: this.AddressForm.PostCode,
										IsClosed: this.AddressForm.IsClosed
									},
									url: _this.$store.state.urlConfigs.AddFamilyAddress,
									success: function(data) {
										if(data.data) {
											_this.$Message.success('添加成功!');
										} else {
											_this.$Message.error('添加失败');
										}
										_this.$Loading.finish();
										_this.SearchAddress(_this.CurrentFamilyID);
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
										ID:this.AddressForm.ID,
										FamilyID: this.CurrentFamilyID,
										Contacter: this.AddressForm.Contacter,
										Phone: this.AddressForm.Phone,
										Address: this.AddressForm.Address,
										PostCode: this.AddressForm.PostCode,
										IsClosed: this.AddressForm.IsClosed
									},
									url: _this.$store.state.urlConfigs.UpdateFamilyAddress,
									success: function(data) {
										if(data.data) {
											_this.$Message.success('修改成功!');
										} else {
											_this.$Message.error('修改失败');
										}
										_this.$Loading.finish();
										_this.SearchAddress(_this.CurrentFamilyID);
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('修改失败!');
									}
								});
							}
						} else {
							this.$Message.error('表单验证失败!');
							//							this.$refs['AddressForm'].resetFields();
						}
					})

				},
				OkFamily() { //TODO：现在只是模拟添加和更新
					this.$refs['FamilyForm'].validate((valid) => {
						var _this = this;
						if(valid) {
							_this.$Loading.start();
							_this.btnSave = true;
							if(this.FamilyIsAdd) {
								_this.ajax({
									type: 'post',
									params: {
										Name: this.FamilyForm.Name,
										Description: this.FamilyForm.Description,
										Mobile: this.FamilyForm.Mobile,
										Region: this.FamilyForm.Region,
										FamilyNumber: this.FamilyForm.FamilyNumber,
										IsClosed: this.FamilyForm.IsClosed
									},
									url: _this.$store.state.urlConfigs.AddFamilyInfo,
									success: function(data) {
										if(data.data.FamilyError === 1) {
											this.CurrentFamilyID = data.data.FamilyID;
											this.FamilyIsAdd = false;
											this.FamilyTitle = '修改家庭信息'
											this.$Message.success('保存家庭信息成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.searchFamily();
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
										ID: this.CurrentFamilyID,
										Name: this.FamilyForm.Name,
										Description: this.FamilyForm.Description,
										Mobile: this.FamilyForm.Mobile,
										Region: this.FamilyForm.Region,
										FamilyNumber: this.FamilyForm.FamilyNumber,
										IsClosed: this.FamilyForm.IsClosed
									},
									url: _this.$store.state.urlConfigs.UpdateFamilyInfo,
									success: function(data) {
										if(data.data.FamilyError === 1) {
											this.$Message.success('修改成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.searchFamily();
									},
									error: function() {
										_this.$Loading.error();
										_this.$Message.error('修改成功!');
									}
								});
							}
						} else {
							this.$Message.error('表单验证失败!');
							//							this.$refs['FamilyForm'].resetFields();
						}
					})
				},
				show(index) {
					var _this = this;
					var row = _this.fdata[index];
					_this.CurrentFamilyID = row.ID;
					_this.address = `${row.Name}的配送地址`;
					_this.$Loading.start();
					_this.ajax({
						type: 'get',
						url: _this.$store.state.urlConfigs.GetFamilyAddressByFamilyID,
						params: {
							familyId: row.ID
						},
						success: function(data) {
							_this.modal1 = true;
							_this.addressData = data.data.Result;
							_this.$Loading.finish();
						},
						error: function() {
							_this.$Loading.error();
							_this.$Message.error('查询失败!');
						}
					})
				},
				AddNewAddress() {
					if(this.CurrentFamilyID === 0) {
						this.$Message.warning('请先保存家庭信息!');
						return;
					}
					this.AddressTitle = '添加新地址';
					this.modal2 = true;
					this.AddressIsAdd = true;
					this.$refs['AddressForm'].resetFields();
				},
				UpdateAddress(index) {
					var row = this.addressData[index];
					this.AddressCurrentRow = index;
					this.AddressTitle = '修改地址';
					this.AddressIsAdd = false;
					this.modal2 = true;
					this.$set(this.AddressForm, 'ID', row.ID);
					this.$set(this.AddressForm, 'Contacter', row.Contacter);
					this.$set(this.AddressForm, 'IsClosed', row.IsClosed + '');
					this.$set(this.AddressForm, 'Phone', row.Phone);
					this.$set(this.AddressForm, 'Address', row.Address);
					this.$set(this.AddressForm, 'PostCode', row.PostCode);
					this.$refs['AddressForm'].validate((valid) => {});
				},
				AddFamily() {
					this.CurrentFamilyID = 0;
					this.FamilyTitle = '添加家庭信息'
					this.FamilyIsAdd = true;
					this.modalFamily = true;
					this.$set(this.FamilyForm, 'ID', 0);
					this.$set(this.FamilyForm, 'Name', '');
					this.$set(this.FamilyForm, 'Description', '');
					this.$set(this.FamilyForm, 'Mobile', '');
					this.$set(this.FamilyForm, 'Region', '');
					this.$set(this.FamilyForm, 'FamilyNumber', 1);
					this.$set(this.FamilyForm, 'FamilyAddress', []);
					this.$set(this.FamilyForm, 'IsClosed', 'false');
					this.addressData = this.FamilyForm.FamilyAddress;
					//					this.$refs['AddressForm'].resetFields();
					this.$refs['FamilyForm'].resetFields();
				},
				UpdateFamily(index) {
					var row = this.fdata[index];
					this.FamilyCurrentRow = index;
					this.CurrentFamilyID = row.ID;
					this.FamilyTitle = '修改家庭信息'
					this.FamilyIsAdd = false;
					this.modalFamily = true;
					this.$set(this.FamilyForm, 'ID', row.ID);
					this.$set(this.FamilyForm, 'Name', row.Name);
					this.$set(this.FamilyForm, 'Description', row.Description);
					this.$set(this.FamilyForm, 'Mobile', row.Mobile);
					this.$set(this.FamilyForm, 'Region', row.Region);
					this.$set(this.FamilyForm, 'FamilyNumber', row.FamilyNumber);
					this.$set(this.FamilyForm, 'IsClosed', row.IsClosed + '');
					this.$refs['FamilyForm'].validate((valid) => {});
					this.SearchAddress(row.ID);
				},
				searchFamily(changePage) {
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
						url: _this.$store.state.urlConfigs.GetFamilyInfoList,
						type: 'post',
						params: _this.Search,
						success: function(data) {
							_this.fdata = data.data.Result;
							_this.count = data.data.TotalCount
							_this.$Loading.finish();
						},
						error: function() {
							_this.$Loading.error();
							_this.$Message.error('查询失败!');
						}
					});
				},
				SearchAddress(id) {
					var _this = this;
					_this.$Loading.start();
					_this.ajax({
						type: 'get',
						url: _this.$store.state.urlConfigs.GetFamilyAddressByFamilyID,
						params: {
							familyId: id
						},
						success: function(data) {
							_this.addressData = data.data.Result;
							_this.$Loading.finish();
						},
						error: function() {
							_this.$Loading.error();
							_this.$Message.error('查询失败!');
						}
					})
				},
				ChangePage(p) {
					this.currentPage = p;
					this.searchFamily(true);
				}
			},
			created() {
				this.searchFamily();
			}
	}
</script>