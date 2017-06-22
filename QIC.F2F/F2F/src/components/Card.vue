<template>
	<div>
		<div class="search-bar">
			<Form ref="Search" :model="Search" inline>
				<Form-item prop="Name">
					<Input type="text" v-model="Search.Name" placeholder="卡名称" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="IsClosed" style="width:100px;">
					<Select v-model="Search.IsClosed" clearable placeholder="是否关闭">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Button type="primary" icon="ios-search" @click="SearchCard">搜索</Button>
				<Button type="primary" icon="plus-round" @click="ShowAddCard">添加</Button>
			</Form>
		</div>
		<Table border :context="self" :columns="columns" :data="cardData"></Table>
		<Page :total="count" :page-size="size" :current="currentPage" show-total show-elevator @on-change="ChangePage" style="padding:10px"></Page>
		<Modal v-model="CardModal"  :title="CardTitle" cancel-text='' width='auto' @on-ok="OkCard()" :styles="CardStyle">
			<Form ref="CardForm" :model="CardForm" :label-width="80" :rules="Rule">
				<Form-item label="名称" prop="Name">
					<Input v-model="CardForm.Name" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="描述" prop="Description">
					<Input v-model="CardForm.Description" type="textarea" :autosize="{minRows: 2,maxRows: 6}" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="配送次数" prop="TotalCount">
					<Input-number v-model="CardForm.TotalCount" :min="0" placeholder="请输入"></Input-number>
				</Form-item>
				<Form-item label="是否关闭" prop="IsClosed">
					<Select v-model="CardForm.IsClosed" placeholder="请选择">
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
				Search: {
					Name: '',
					IsClosed: ''
				},
				self: this,
				count: 0,
				size: 10,
				currentPage: 1,
				CardTitle: ' ',
				CardModal: false,
				CardStyle: {
					padding: '20px',
					maxWidth: '600px'
				},
				IsAdd: true,
				CardForm: {
					ID:0,
					Name: '',
					Description: '',
					TotalCount: 0,
					IsClosed: 'false'
				},
				Rule: {
					Name: [{
						required: true,
						message: '请填写卡名称',
						trigger: 'blur'
					}],
					Description: [{
						required: true,
						message: '请填写描述',
						trigger: 'blur'
					}]
				}, //信息验证规则
				cardData: [],
				columns: [{
					title: '名称',
					key: 'Name',
					align: 'center'
				}, {
					title: '配送次数',
					key: 'TotalCount',
					align: 'center'
				}, {
					title: '描述',
					align: 'center',
					key: 'Description',
					width: '600'
				}, {
					title: '是否关闭',
					key: 'IsClosed',
					align: 'center',
					render(row, column, index) {
						if((row.IsClosed + '') == 'true') return '是';
						return '否';
					}
				}, {
					title: '操作',
					key: 'Operate',
					align: 'center',
					width: "100",
					render(row, column, index) {
						return `<i-button type='primary' @click="ShowUpdateCard(${index})"> 编辑 </i-button>`;
					}
				}]
			}
		}, watch: {

		}, methods: {
			OkCard() {
				this.$refs['CardForm'].validate((valid) => {
					var _this = this;
					if(valid) {
						if(_this.IsAdd) {
							_this.$Loading.start();
							_this.ajax({
									type: 'post',
									params: {										
										Name:_this.CardForm.Name,
										Description:_this.CardForm.Description,
										TotalCount:_this.CardForm.TotalCount,
										IsClosed:_this.CardForm.IsClosed
									},
									url: _this.$store.state.urlConfigs.AddCardKind,
									success: function(data) {
										if(data.data.CardKindError===1) {
											_this.$Message.success('添加成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchCard();
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
										ID:_this.CardForm.ID,
										Name:_this.CardForm.Name,
										Description:_this.CardForm.Description,
										TotalCount:_this.CardForm.TotalCount,
										IsClosed:_this.CardForm.IsClosed
									},
									url: _this.$store.state.urlConfigs.UpdateCardKind,
									success: function(data) {
										if(data.data.CardKindError===1) {
											_this.$Message.success('修改成功!');
										} else {
											_this.$Message.error(data.data.Message);
										}
										_this.$Loading.finish();
										_this.SearchCard();
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
			},
			ShowUpdateCard(index) {
				var row = this.cardData[index];
				this.CardTitle = '编辑卡类型';
				this.CardModal = true;
				this.IsAdd = false;
				this.$set(this.CardForm, 'ID', row.ID);
				this.$set(this.CardForm, 'Name', row.Name);
				this.$set(this.CardForm, 'Description', row.Description);
				this.$set(this.CardForm, 'TotalCount', row.TotalCount);
				this.$set(this.CardForm, 'IsClosed', row.IsClosed + '');
				this.$refs['CardForm'].validate((valid) => {});
			},
			ShowAddCard() {
				this.CardTitle = '添加卡类型';
				this.CardModal = true;
				this.IsAdd = true;
				this.$refs["CardForm"].resetFields();
			},
			SearchCard(changePage) {
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
					url: _this.$store.state.urlConfigs.GetCardKindList,
					type: 'post',
					params: _this.Search,
					success: function(data) {
						_this.cardData = data.data.Result;
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
				this.currentPage = p;
				this.SearchCard(true);
			}

		},
		created(){
			this.SearchCard();
		}
	}
</script>