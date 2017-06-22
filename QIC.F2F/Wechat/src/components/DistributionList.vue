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
					<div class="border_first first">
						<div class="margin3">配送时间: <span class="txt-green">{{list.DistributedDateStr}}</span></div>						
						<div class="margin3" v-for="f in list.FamilyCardInfo">{{f.CardKindName}}，共{{f.TotalCount}}次，第{{f.DistributedCount}}次</div>
					</div>
					<div class="borderlist">
						<div class="txt-list-number">序号</div>
						<div class="txt-list-title">品种</div>
						<div class="txt-list-title">数量</div>
						<div class="txt-list-title">溯源报告</div>
						<div class="txt-list-title">营养美食</div>
					</div>
					<template v-for="(d,index) in list.DistributionDetails">
						<div class="border">
							<div class="txt-item-number">{{index+1}}</div>
							<div class="txt-item">{{d.ProductName}}</div>
							<div class="txt-item">{{d.Amount}}{{d.Unit}}</div>
							<div class="txt-item"><a :href="$store.state.Area+'/'+d.ProductReportUrl" class="report"><i class="i-report"></i></a></div>
							<div class="txt-item"><a :href="$store.state.Area+'/'+d.ProductCookingUrl" class="report"><i class="i-cooking"></i></a></div>							
							
							
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
	.item .border_first{
		border-bottom: 0.1px solid #B2B2B2;
		padding: 10px;
		position: relative;
	}
	.item .border {
		border-bottom: 0.1px solid #B2B2B2;		
		padding: 5px 10px 0 10px;
		display: flex;
		justify-content: space-around;
	}
	.item .border:nth-of-type(2n){
		background:#f7f7f7 ;
	}
	.item .border .txt-item{
		text-align: center;
		width:20%;
		padding-top:5px;
	}
	.item .border .txt-item-number{	
		padding-top:5px;
		padding-left:2.5%;			
		width:5%;
		padding-right: 2.5%;
	}
	.item .borderlist{
		display: flex;
		justify-content: space-around;
		padding: 10px;
		border-bottom: 0.1px solid #B2B2B2;
		background: #f7f7f7;
	}
	.borderlist .txt-list-title{
		color: #333333;
		font-weight: 600;
		width:20%;
		text-align: center;
		
	}
	.borderlist .txt-list-number{
		color: #333333;		
		font-weight: 600;
		width:10%;				
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
	
	.report {
		text-decoration: none;
		color: black;
	}
	.report .i-cooking{
		display: inline-block;
		width: 16px;
		height: 24px;
		background: url("../static/img/cooking.png") no-repeat;
		background-size: 100%;
	}
	.report .i-report{
		display: inline-block;
		width: 16px;
		height: 24px;
		background: url("../static/img/report.png") no-repeat;
		background-size: 100%;
	}
</style>