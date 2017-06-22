<template>
	<div style="100%;text-align: center;">
		<form class="bindingForm">
			<label>手机号</label><input type="text" v-model="PhoneValue" placeholder="请输入手机号" class="text">
		</form>
		<span class="phoneMsg" v-show="ShowMsg">{{PhoneMsg}}</span>
		<button class="btnSubmit" :disabled="BtnDis" @click="CheckPhone">{{BtnTitle}}</button>
	</div>
</template>
<script>
	export default {
		
		data() {
				return {
					name: 'a',
					PhoneMsg: ' ',
					msg: '无任何数据，请与工作人员联系',
					ShowMsg: true,
					PhoneValue: '',
					BtnDis: false,
					BtnTitle: '提交'
				}
			},
			methods: {
				CheckPhone() {
					var re = /^1\d{10}$/;
					if(!re.test(this.PhoneValue)) {
						this.PhoneMsg = '请输入正确的手机号';
						this.PhoneValue = '';
						return;
					} else {
						this.PhoneMsg = '';
						this.BtnDis = true;
						this.BtnTitle = '提交中...'
						this.ajax({
							url: this.$store.state.urlConfigs.GetUserInfoByPhoneNumber,
							params: {
								phoneNumber: this.PhoneValue
							},
							type: 'get',
							success(data) {
								if(data.data === null) {
									this.PhoneMsg = this.msg;
								} else {
									this.$router.push({
										name: 'UserInfo',
										query: {
											Name: data.data.Name,
											Mobile: data.data.Mobile
										}
									});
								}
								this.BtnDis = false;
								this.BtnTitle = '提交'
							},
							error() {
								alert('系统繁忙！');
								this.BtnDis = false;
								this.BtnTitle = '提交'
							}
						});
					}

				}
			},
			created() {
				//				var _this = this;
				//				setTimeout(() => {
				//					_this.ShowMsg = true;
				//				}, 2000);
			}
	}
</script>
<style scoped>
	.bindingForm {
		margin-top: 20px;
		display: flex;
		padding-left: 24px;
	}
	
	.bindingForm label {
		display: inline-block;
		width: 60px;
		flex-grow: 0;
		height: 36px;
		line-height: 42px;
		text-align: left;
	}
	
	.bindingForm .text {
		height: 36px;
		margin-right: 30px;
		display: inline-block;
		padding-left: 10px;
		line-height: 36px;
		flex-grow: 1;
		border-radius: 10px;
		border: 0.1px solid #B2B2B2;
		outline: none;
		-webkit-appearance: none;
	}
	
	.phoneMsg {
		display: block;
		font-size: 14px;
		transform: scale(0.8);
		color: red;
		margin-left: 90px;
		text-align: left;
		height: 20px;
		line-height: 20px;
	}
	
	.btnSubmit {
		width: 120px;
		height: 36px;
		color: white;
		background: #0aa770;
		border: none;
		border-radius: 10px;
		outline: none;
	}
</style>