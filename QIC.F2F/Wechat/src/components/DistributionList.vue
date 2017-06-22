<template>
	<div>
		<scroller :on-refresh="refresh" :on-infinite="infinite" ref="scoller">
			<div class="header">
				 <div><span class="txt-green"> {{account}}</span>会员，您好！</div>
				 <div class="txt-gray">欢迎来到畦溪源会员服务系统。</div>
			</div>      
			<div class="banner">
				<div class="dot_item"></div><div class="txt-record">配送记录</div><div class="dot_item"></div>
			</div>
			<template v-for="list in ListData">
				<div class="item">
					<div class="border first">
						<div class="margin3">配送时间: <span class="txt-green">{{list.DistributedDateStr}}</span></div>						
						<div class="margin3" v-for="f in list.FamilyCardInfo">{{f.CardKindName}}，共{{f.TotalCount}}次，第{{f.DistributedCount}}次</div>
					</div>
					<template v-for="d in list.DistributionDetails">
						<div class="border">
							<a :href="$store.state.Area+'/'+d.ProductCookingUrl" class="name"><span>{{d.ProductName}}</span> <span class="name-unit">{{d.Amount}}{{d.Unit}}</span><i class="name-i">></i></a>
							<a :href="$store.state.Area+'/'+d.ProductReportUrl" class="report">溯源报告</a>
						</div>
					</template>
				</div>
			</template>
		</scroller>
	</div>
</template>
<script>
	export default {
		data() {
				var openID = sessionStorage["OpenID"]
				return {
					ListData: [],
					page: 1,
					size: 10,
					currentPage: 1,
					type: 0,
					count: 0,
					account:''
				}
			},
			methods: {
				refresh() {
					this.allowinfinite = true;
					this.page = 1;
					this.type = 0;
					this.SearchList();

				},
				infinite() {
					var _this = this;
					this.page++;
					this.type = 1;
					this.SearchList();

				},
				SearchList() {
					var _this = this;
					this.ajax({
						url: this.$store.state.urlConfigs.GetDistributionListByOpenID,
						type: 'post',
						params: {
							OpenID: sessionStorage["OpenID"],
							PageIndex: this.page,
							PageSize: this.size
						},
						success(data) {
							this.count = data.data.Result.length;
							if(this.type == 0) {
								this.ListData = [];
								setTimeout(function() {
									_this.$refs["scoller"].finishPullToRefresh()
								}, 500);
							} else {
								var nodata = false;
								if(this.count == 0) {
									nodata = true;
								}
								setTimeout(function() {
									_this.$refs["scoller"].finishInfinite(nodata)
								}, 500);
							}
							this.account=data.data.MemberName;
							this.ListData = this.ListData.concat(data.data.Result);
						},
						error() {													
							alert("系统繁忙");
						}
					})
				}
			},
			created() {
				this.$store.commit('UpdateHeader');
				this.SearchList();
				sessionStorage["Back"]='Back';
			}
	}
</script>
<style scoped>
	.header{
		padding-top:10px;
		text-align:center;
		font-size: 12px;
		color:gray;
	}
	.txt-green{
		color:#0AA770;
	}
	.header .txt-green{
		color:#0AA770;
		margin-right: 5px;
	}
	.header .txt-gray{
		color:gray;
	}
	.banner{
		font-weight: bold;
		text-align: center;
		padding:10px;				
		display: flex;
	}
	.banner .dot_item{
		height: 1px;
		border-top: dotted 1px #B2B2B2;
		width:10px;
		margin: 10px;
		flex-grow: 1;	
		font-size:12px;	
	}
	.banner .txt-record{
		flex-grow:0;
	}
	.item {
		margin-bottom: 10px;
		background: white;
		font-size: 12px;
	}
	
	.item .border {
		border-bottom: 0.1px solid #B2B2B2;
		padding: 10px;
		position: relative;
	}
	
	.item .first {
		border-top: 0.1px solid #B2B2B2;
	}
	.item .first .margin3{
		margin-bottom:3px;
	}
	.time {
		border-left: #0aa770 3px solid;
		color: #0AA770;
		padding-left: 10px;		
	}
	
	.name {
		height: 30px;
		line-height: 30px;
		display: inline-block;
		width: 100%;
		text-decoration: none;
		color: black;
	}
	
	.name .name-i {
		position: absolute;
		right: 120px;
		color: #808080;
	}
	
	.name .name-unit {
		position: absolute;
		right: 140px;
	}
	
	.report {
		position: absolute;
		display: inline-block;
		padding: 15px;
		height: 30px;
		line-height: 30px;
		right: 20px;
		top: 8px;
		text-decoration: none;
		color: black;
		border: #B2B2B2 solid 0.5px;
		background: #EAEAEA;
		padding: 0 6px;
		border-radius: 6px;
		font-size: 12px;
	}
</style>