<template>
	<div style="background: #1f2d3d;height: 100%;padding:10px;">
		<div id="loginForm">
			<Form ref="formInline" :model="formInline" :rules="ruleInline">
				<Form-item prop="title">
					<h3>系统登录</h3>
				</Form-item>
				<Form-item prop="user">
					<Input type="text" v-model="formInline.user" placeholder="用户名" autocomplete="on">
					<Icon type="ios-person-outline" slot="prepend"></Icon>
					</Input>
				</Form-item>
				<Form-item prop="password">
					<Input type="password" v-model="formInline.password" placeholder="密码" autocomplete="on">
					<Icon type="ios-locked-outline" slot="prepend"></Icon>
					</Input>
				</Form-item>
				<Form-item>
					<Button type="primary" @click="handleSubmit('formInline')">登录</Button>
				</Form-item>
			</Form>
		</div>
	</div>
</template>
<script>
	import router from '@/router'
	export default {
		data() {
				return {
					formInline: {
						user: '',
						password: ''
					},
					ruleInline: {
						user: [{
							required: true,
							message: '请填写用户名',
							trigger: 'blur'
						}],
						password: [{
							required: true,
							message: '请填写密码',
							trigger: 'blur'
						}, {
							type: 'string',
							min: 6,
							message: '密码长度不能小于6位',
							trigger: 'blur'
						}]
					}
				}
			},
			methods: {
				handleSubmit(name) {
					var _this = this;
					this.$refs[name].validate((valid) => {
						if(valid) {
							_this.$Loading.start();
							this.ajax({
								url: this.$store.state.urlConfigs.Login,
								type: 'get',
								params: {
									password: this.formInline.password,
									username: this.formInline.user
								},
								success: function(data) {								
									if(data.data.LoginError===1) {
										this.$store.commit('Login', this.formInline);
										router.push({
											path: '/Home'
										})
										_this.$Loading.finish();
										this.$Message.success('登录成功!');
									} else {
										_this.$Loading.error();
										this.$Message.error(data.data.Message);
									}
								},
								error: function() {
									_this.$Loading.error();
									this.$Message.error('系统错误!');
								}
							});

						} else {
							this.$Message.error('表单验证失败!');
						}
					})
				}
			}
	}
</script>
<style scoped>
	#loginForm {
		background: white;
		width: 300px;
		height: auto;
		margin: 150px auto;
		padding: 30px;
		padding-bottom: 20px;
		border: thin solid #b2b2b2;
		border-radius: 8px;
	}
</style>