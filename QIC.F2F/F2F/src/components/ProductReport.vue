<template>
	<div>
		<div class="search-bar">
			<Form ref="Search" :model="Search" inline>
				<Form-item prop="Name">
					<Input type="text" v-model="Search.Name" placeholder="报告名称" autocomplete="off">
					</Input>
				</Form-item>
				<Form-item prop="IsExpired" style="width:100px;">
					<Select v-model="Search.IsExpired" clearable placeholder="是否关闭">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Form-item prop="IsClosed" style="width:100px;">
					<Select v-model="Search.IsClosed" clearable placeholder="是否关闭">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Button type="primary" icon="ios-search" @click="SearchProductReport()">搜索</Button>
				<Button type="primary" icon="plus-round" @click="ShowAddProductReport">添加</Button>
			</Form>
		</div>
		<Table border :context="self" :columns="columns" :data="ProductReportData"></Table>
		<Page :total="count" :page-size="size" :current="currentPage" show-total show-elevator @on-change="ChangePage" style="padding:10px"></Page>
		<Modal v-model="ProductReportModal" @on-ok="OkProductReport" :title="ProductReportTitle" cancel-text='' width='auto' :styles="ProductReportStyle">
			<Form ref="ProductReportForm" :model="ProductReportForm" :label-width="120" :rules="Rule">
				<Form-item label="报告名称" prop="Name" v-show="!IsScan" >
					<Input v-model="ProductReportForm.Name" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="报告描述" prop="Description" v-show="!IsScan" >
					<!--<quill-editor ref="myTextEditor" v-model="ProductReportForm.Description" :config="editorOption" >
					</quill-editor>-->
					<div class="editer" style="width:300px;"><vue-editor v-model="ProductReportForm.Description" ></vue-editor></div>
					<!--<Input v-model="ProductReportForm.Description" type="textarea" placeholder="请输入"></Input>-->
				</Form-item>
				<Form-item prop="Description" v-show="IsScan">
					<div v-html="ProductReportForm.Description">{{ProductReportForm.Description}}</div>
				</Form-item>
				<Form-item label="制作人" prop="Writer" v-show="!IsScan">
					<Input v-model="ProductReportForm.Writer" placeholder="请输入"></Input>
				</Form-item>
				<Form-item label="是否过期" prop="IsExpired" v-show="!IsScan">
					<Select v-model="ProductReportForm.IsExpired" placeholder="请选择">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
				<Form-item label="是否关闭" prop="IsClosed" v-show="!IsScan">
					<Select v-model="ProductReportForm.IsClosed" placeholder="请选择">
						<Option value="true">是</Option>
						<Option value="false">否</Option>
					</Select>
				</Form-item>
			</Form>

		</Modal>
	</div>
</template>
<script>
	import { VueEditor } from 'vue2-editor'
	export default {
		components: {
			VueEditor
		},
		data() {
			var _this = this;
			return {
				content: '<h2>I am Example</h2>',
				editorOption: {
					// something config
				},
				Search: {
					Name: '',
					IsClosed: '',
					IsExpired: ''
				},
				self: this,
				count: 0,
				size: 10,
				currentPage: 1,
				ProductReportTitle: ' ',
				ProductReportModal: false,
				ProductReportStyle: {
					padding: '20px',
					maxWidth: '1000px'
				},
				IsAdd: true,
				IsScan: false,
				ProductReportForm: {
					ID: 0,
					Name: '',
					Description: '',
					Writer: '',
					IsExpired: 'false',
					IsClosed: 'false',
				},
				Rule: {
					Name: [{
						required: true,
						message: '请填写报告名称',
						trigger: 'blur'
					}],
					Description: [{
						required: true,
						message: '请填写描述',
						trigger: 'blur'
					}],
					Writer: [{
						required: true,
						message: '请填写制作人',
						trigger: 'blur'
					}]
				}, //信息验证规则
				ProductReportData: [],
				columns: [{
					title: '报告名称',
					key: 'Name',
					align: 'center'
				}, {
					title: '制作人',
					align: 'center',
					key: 'Writer'
				}, {
					title: '是否过期',
					key: 'IsExpired',
					align: 'center',
					render(row, column, index) {
						if((row.IsExpired + '') == 'true') return '是';
						return '否';
					}
				}, {
					title: '是否关闭',
					key: 'IsClosed',
					align: 'center',
					render(row, column, index) {
						if((row.IsClosed + '') == 'true') return '是';
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
						var url = _this.$store.state.Area + '/' + row.Description;
						return `<a  href=${url} target="_blank" class="btn_scan" > 浏览 </a> <i-button type='primary' @click="ShowUpdateProductReport(${index})"> 编辑 </i-button>`;
					}
				}]
			}
		},
		watch: {},
		methods: {
			onEditorBlur(editor) {
				console.log('editor blur!', editor)
			},
			onEditorFocus(editor) {
				console.log('editor focus!', editor)
			},
			onEditorReady(editor) {
				console.log('editor ready!', editor)
			},
			onEditorChange({
				editor,
				html,
				text
			}) {
				// console.log('editor change!', editor, html, text)
				this.content = html
			},
			OkProductReport() {
				if(this.IsScan) return;
				this.$refs['ProductReportForm'].validate((valid) => {
					var _this = this;
					if(valid) {
						_this.$Loading.start();
						if(_this.IsAdd) {

							_this.ajax({
								type: 'post',
								params: {
									Name: _this.ProductReportForm.Name,
									Description: _this.ProductReportForm.Description,
									Writer: _this.ProductReportForm.Writer,
									IsExpired: _this.ProductReportForm.IsExpired,
									IsClosed: _this.ProductReportForm.IsClosed
								},
								url: _this.$store.state.urlConfigs.AddProductReport,
								success: function(data) {
									if(data.data.ProductReportError === 1) {
										_this.$Message.success('添加成功!');
									} else {
										_this.$Message.error(data.data.Message);
									}
									_this.$Loading.finish();
									_this.SearchProductReport();
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
									ID: _this.ProductReportForm.ID,
									Name: _this.ProductReportForm.Name,
									Description: _this.ProductReportForm.Description,
									Writer: _this.ProductReportForm.Writer,
									IsExpired: _this.ProductReportForm.IsExpired,
									IsClosed: _this.ProductReportForm.IsClosed
								},
								url: _this.$store.state.urlConfigs.UpdateProductReport,
								success: function(data) {
									if(data.data.ProductReportError === 1) {
										_this.$Message.success('修改成功!');
									} else {
										_this.$Message.error(data.data.Message);
									}
									_this.$Loading.finish();
									_this.SearchProductReport();
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
			ShowUpdateProductReport(index) {
				var _this = this;
				_this.$Loading.start();
				var row = this.ProductReportData[index];
				this.ProductReportTitle = '编辑报告';
				this.$set(this.ProductReportForm, 'ID', row.ID);
				this.$set(this.ProductReportForm, 'Name', row.Name);
				this.$set(this.ProductReportForm, 'Writer', row.Writer);
				this.$set(this.ProductReportForm, 'IsExpired', row.IsExpired + '');
				this.$set(this.ProductReportForm, 'IsClosed', row.IsClosed + '');
				this.IsAdd = false;
				this.IsScan = false;
				_this.ajax({
					url: _this.$store.state.urlConfigs.GetProductReportStr,
					type: 'get',
					params: {
						id: _this.ProductReportForm.ID
					},
					success(data) {						
						_this.$set(this.ProductReportForm, 'Description', data.data);
						_this.$Loading.finish();
						_this.ProductReportModal = true;
						_this.$refs['ProductReportForm'].validate((valid) => {});
					},
					error() {
						_this.$Loading.error();
						_this.$Message.error('获取溯源报告内容失败!');
					}
				});
			},
			ShowAddProductReport() {
				this.ProductReportTitle = '添加报告';
				this.ProductReportModal = true;
				this.IsAdd = true;
				this.IsScan = false;
				this.$refs['ProductReportForm'].resetFields();
			},
			SearchProductReport(changePage) {
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
					url: _this.$store.state.urlConfigs.GetProductReportList,
					type: 'post',
					params: _this.Search,
					success: function(data) {
						_this.ProductReportData = data.data.Result;
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
				this.SearchProductReport(true);
			},
			ScanProductReport(index) {
				this.IsScan = true;
				var row = this.ProductReportData[index];
				this.ProductReportTitle = '浏览报告';
				this.ProductReportModal = true;
				this.$set(this.ProductReportForm, 'Description', row.DescriptionHtmlContent);
			}

		},
		created() {
			this.SearchProductReport();
		}
	}
</script>
<style>	
	.btn_scan {
		background-color: #3091f2;
		border-color: #3091f2;
		color: white;
		font-weight: 400;
		text-align: center;
		border: 1px solid transparent;
		white-space: nowrap;
		line-height: 1.5;
		padding: 7px 15px 9px 15px;
		margin-right: 5px;
		font-size: 12px;
		border-radius: 4px;
	}
	
	.btn_scan:hover {
		color: white;
		background-color: #5cadff;
		border-color: #5cadff;
	}
</style>