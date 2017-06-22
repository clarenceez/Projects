<template>
	<div style="padding:20px;text-align: center;">
		{{Tip}}
	</div>
</template>

<script>
	export default {
		data() {
			return {
				Tip: '页面跳转中...'
			}

		},
		created() {
			if(sessionStorage["Back"] === 'Back') {
				this.$router.push({
					name: 'DistributionList'
				})
			}
			var r = new RegExp("(\\?|#|&)code=([^&#]*)(&|#|$)");
			var m = location.href.match(r);
			var code = decodeURIComponent(!m ? "" : m[2]);
			this.ajax({
				url: this.$store.state.urlConfigs.Login,
				params: {
					code: code
				},
				type: 'get',
				success(data) {					
						if(data.data.WechatLoginError !== 1) {
							this.Tip = '系统繁忙';
							return;
						}
					sessionStorage["OpenID"] = data.data.OpenID;
					sessionStorage["Token"] = data.data.Token;
					this.$store.commit('UpdateHeader');
					if(data.data.Token === null) {
						this.$router.push({
							name: 'BindingPhone'
						})
					} else {
						this.$router.push({
							name: 'DistributionList'
						})
					}
				},
				error() {
					alert("系统繁忙");
				}
			})
		}
	}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->