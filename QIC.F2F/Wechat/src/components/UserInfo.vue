<template>
	<div style="padding-left:26px;padding-right: 30px;">
		<div><label class="infoLabel">手机号：</label>{{$route.query.Mobile}} </div>
		<div><label class="infoLabel">姓名：</label>{{$route.query.Name}}</div>
		<div class="userHr"></div>
		<div style="text-align: center;">
			<form class="captchaForm">
				<label>验证码</label><input type="text" v-model="CaptchaValue" class="text" placeholder="请输入验证码"><button class="btnCaptcha" :disabled="BtnCaptchaDis" @click="GetCaptcha">{{CaptChaTitle}}</button>
			</form>
			<span class="captchaMsg">{{CaptchaMsg}}</span>
			<button class="btnSubmit" :disabled="BtnDis" @click="CheckCaptcha">{{BtnTitle}}</button>
		</div>
	</div>

</template>
<script>
	export default {
		created() {
			if(sessionStorage["Back"] === 'Back') {
				this.$router.push({
					name: 'DistributionList'
				})
			}
		},
		data() {
			return {
				CaptChaTitle: '获取验证码',
				CaptchaMsg: '',
				BtnTitle: '提交',
				CaptchaValue: '',
				BtnDis: false,
				BtnCaptchaDis: false,
				Timer: 0
			};
		},
		methods: {
			CheckCaptcha() {
				if(this.CaptchaValue != '') {
					this.CaptchaMsg = '';
					this.BtnDis = true;
					this.BtnTitle = '提交中...';
					this.ajax({
						url: this.$store.state.urlConfigs.BindUser,
						params: {
							OpenID: sessionStorage["OpenID"],
							PhoneNumber: this.$route.query.Mobile,
							Captcha: this.CaptchaValue
						},
						type: 'post',
						success(result) {
							if(result.data.BindUserError === 1) {
								sessionStorage["Token"] = result.data.Token;
								this.$store.commit('UpdateHeader');
								this.BtnDis = false;
								this.BtnTitle = '提交';
								this.$router.push({
									name: 'DistributionList'
								})
							} else {
								this.CaptchaMsg = result.data.Message;
								this.BtnDis = false;
								this.BtnTitle = '提交';
								this.CaptChaTitle = '获取验证码';
								this.BtnCaptchaDis = false;
								clearInterval(this.Timer);
							}
						},
						error() {
							alert('系统繁忙');
							this.BtnDis = false;
							this.BtnTitle = '提交';
						}
					});
				} else {
					this.CaptchaMsg = '验证码为空';
				}
			},
			GetCaptcha() {
				var re = /^1\d{10}$/;
				if(!re.test(this.$route.query.Mobile)) {
					this.CaptchaMsg = '手机号格式不正确';
					return;
				} else {
					this.CaptchaMsg = '';
					this.BtnCaptchaDis = true;
					this.CaptChaTitle = '获取中...';
					this.ajax({
						url: this.$store.state.urlConfigs.GetMobileCaptChaForBindUser,
						params: {
							phoneNumber: this.$route.query.Mobile
						},
						type: 'get',
						success(result) {								
							if(result.data.Data == null) {
								this.CaptchaMsg = '获取验证码失败';
								this.CaptChaTitle = '获取验证码';
								this.BtnCaptchaDis = false;
							} else {
								var count = this.$store.state.CaptchaTime;
								var _this = this;
								this.Timer = setInterval(function() {
									count--;
									if(count > 0) {
										_this.CaptChaTitle = `${count}秒`;
									} else {
										_this.CaptChaTitle = '获取验证码';
										_this.BtnCaptchaDis = false;
										clearInterval(_this.Timer);
									}
								}, 1000);

							}

						},
						error() {
							alert('系统繁忙');
							this.BtnCaptchaDis = false;
							this.CaptChaTitle = '获取验证码';
						}
					})
				}
			}
		}
	}
</script>
<style scoped>
	.infoLabel {
		display: inline-block;
		margin-right: 10px;
		height: 36px;
		line-height: 36px;
		text-align: left;
	}
	
	.userHr {
		border-top: 0.1px solid #B2B2B2;
		margin-bottom: 20px;
	}
	
	.captchaForm {
		margin-top: 20px;
		display: flex;
		justify-content: space-around;
	}
	
	.captchaForm label {
		display: inline-block;
		width: 60px;
		flex-grow: 0;
		height: 36px;
		line-height: 36px;
		text-align: left;
		flex-shrink: 0;
	}
	
	.captchaForm .text {
		height: 34px;
		display: inline-block;
		padding-left: 10px;
		line-height: 34px;
		flex-shrink: 1;
		flex-grow: 1;
		border-radius: 10px;
		border: 0.5px solid #b2b2b2;
		outline: none;
		width: 40%;
		-webkit-appearance: none;
	}
	
	.btnCaptcha {
		margin-left: 10px;
		flex-grow: 0;
		width: 100px;
		height: 40px;
		line-height: 40px;
		background: white;
		border: 0.1px solid #B2B2B2;
		border-radius: 10px;
		outline: none;
		flex-shrink: 0;
	}
	
	.captchaMsg {
		display: block;
		font-size: 14px;
		transform: scale(0.8);
		color: red;
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