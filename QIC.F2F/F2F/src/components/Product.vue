<template>
	<div>
		<div class="search-bar">
			<Form ref="Search" :model="Search" inline>
				<Form-item prop="Name">
					<Input type="text" v-model="Search.Name" placeholder="产品名称" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="IsClosed" style="width:100px;">
					<Select v-model="Search.IsClosed" clearable placeholder="是否关闭">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Button type="primary" icon="ios-search" @click="SearchProduct()">搜索</Button>
				<Button type="primary" icon="plus-round" @click="ShowAddProduct">添加</Button>
			</Form>

		</div>
		<Table border :context="self" :columns="columns" :data="productData"></Table>
		<Page :total="count" :page-size="size" :current="currentPage" show-total show-elevator @on-change="ChangePage" style="padding:10px"></Page>
		<Modal v-model="ProductModal" :title="ProductTitle" cancel-text='' @on-ok="OkProduct" width='auto' :styles="ProductStyle">
			<Form ref="ProductForm" :model="ProductForm" :label-width="120" :rules="Rule">
				<Form-item label="名称" prop="Name" v-show="!IsScan">
					<Input v-model="ProductForm.Name" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="描述" prop="Description" v-show="!IsScan">
					<Input v-model="ProductForm.Description" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="单位" prop="Unit" v-show="!IsScan">
					<Input v-model="ProductForm.Unit" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="营养价值以及做法" prop="Cooking" v-show="!IsScan">
					<div class="editer" style="width:300px;"><vue-editor v-model="ProductForm.Cooking"></vue-editor></div>
					<!--<Input v-model="ProductForm.Cooking" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入..."></Input>-->
				</Form-item>
				<Form-item prop="Cooking" v-show="IsScan">
					<div v-html="ProductForm.Cooking">{{ProductForm.Cooking}}</div>
				</Form-item>
				<Form-item label="是否关闭" prop="IsClosed" v-show="!IsScan">
					<Select v-model="ProductForm.IsClosed" placeholder="请选择">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
			</Form>

		</Modal>
	</div>
</template>
<script>
	import {
		VueEditor
	} from 'vue2-editor'
	export default {
		components: {
			VueEditor
		},
		data() {
			var _this = this;
			return {
				Search: {
					Name: '',
					IsClosed: ''
				},
				self: this,
				count: 0,
				size: 10,
				currentPage: 1,
				ProductTitle: ' ',
				ProductModal: false,
				ProductStyle: {
					padding: '20px',
					maxWidth: '1000px'
				},
				IsAdd: true,
				IsScan: false,
				ProductForm: {
					ID: 0,
					Name: '',
					Description: '',
					Unit: '',
					IsClosed: 'false',
					Cooking: ''
				},
				Rule: {
					Name: [{
						required: true,
						message: '请填写产品名称',
						trigger: 'blur'
					}],
					Description: [{
						required: true,
						message: '请填写描述',
						trigger: 'blur'
					}],
					Unit: [{
						required: true,
						message: '请填写单位',
						trigger: 'blur'
					}],
					Cooking: [{
						required: true,
						message: '请填写营养价值和做法',
						trigger: 'blur'
					}]
				}, //信息验证规则
				productData: [],
				columns: [{
						title: '名称',
						key: 'Name',
						align: 'center'
					}, {
						title: '描述',
						align: 'center',
						key: 'Description',
						width: '300'
					}, {
						title: '单位',
						key: 'Unit',
						align: 'center'
					}		
					, {
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
						render(row, column, index) {
							var url = _this.$store.state.Area + '/' + row.Cooking;
							return `<a  href=${url} target="_blank" class="btn_scan" > 浏览 </a>  <i-button type='primary' @click="ShowUpdateProduct(${index})"> 编辑 </i-button>`;
						}
					}
				]
			}
		},
		watch: {

		},
		methods: {
			OkProduct() {
				if(this.IsScan) return;
				this.$refs['ProductForm'].validate((valid) => {
					var _this = this;
					if(valid) {
						if(_this.IsAdd) {
							_this.ajax({
								type: 'post',
								params: {
									Name: _this.ProductForm.Name,
									Description: _this.ProductForm.Description,
									Unit: _this.ProductForm.Unit,
									Cooking: _this.ProductForm.Cooking,
									IsClosed: _this.ProductForm.IsClosed
								},
								url: _this.$store.state.urlConfigs.AddProduct,
								success: function(data) {
									if(data.data.ProductError === 1) {
										_this.$Message.success('添加成功!');
									} else {
										_this.$Message.error(data.data.Message);
									}
									_this.$Loading.finish();
									_this.SearchProduct();
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
									ID: _this.ProductForm.ID,
									Name: _this.ProductForm.Name,
									Description: _this.ProductForm.Description,
									Unit: _this.ProductForm.Unit,
									Cooking: _this.ProductForm.Cooking,
									IsClosed: _this.ProductForm.IsClosed
								},
								url: _this.$store.state.urlConfigs.UpdateProduct,
								success: function(data) {
									if(data.data.ProductError === 1) {
										_this.$Message.success('修改成功!');
									} else {
										_this.$Message.error(data.data.Message);
									}
									_this.$Loading.finish();
									_this.SearchProduct();
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
			ShowUpdateProduct(index) {
				var _this = this;
				_this.$Loading.start();
				var row = this.productData[index];
				this.ProductTitle = '编辑产品类型';				
				this.IsAdd = false;
				this.IsScan = false;
				this.$set(this.ProductForm, 'ID', row.ID);
				this.$set(this.ProductForm, 'Name', row.Name);
				this.$set(this.ProductForm, 'Description', row.Description);
				this.$set(this.ProductForm, 'Unit', row.Unit);
				this.$set(this.ProductForm, 'IsClosed', row.IsClosed + '');						
				_this.ajax({
					url: _this.$store.state.urlConfigs.GetProductCookingStr,
					type: 'get',
					params: {
						id: _this.ProductForm.ID
					},
					success(data) {						
						_this.$set(this.ProductForm, 'Cooking', data.data);
						_this.$Loading.finish();
						_this.ProductModal = true;
						_this.$refs['ProductForm'].validate((valid) => {});
					},
					error() {
						_this.$Loading.error();
						_this.$Message.error('获取做法内容失败!');
					}
				});
			},
			ShowAddProduct() {
				this.ProductTitle = '添加产品类型';
				this.ProductModal = true;
				this.IsAdd = true;
				this.IsScan = false;
				this.$refs['ProductForm'].resetFields();
			},
			SearchProduct(changePage) {
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
					url: _this.$store.state.urlConfigs.GetProductList,
					type: 'post',
					params: _this.Search,
					success: function(data) {
						_this.productData = data.data.Result;
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
				this.SearchProduct(true);
			},
			ScanProduct(index) {
				this.IsScan = true;
				var row = this.productData[index];
				this.ProductTitle = '浏览营养价值和做法';
				this.ProductModal = true;
				this.$set(this.ProductForm, 'Cooking', row.CookingHtmlContent);
			}
		},
		created() {
			this.SearchProduct();
		}
	}
</script>