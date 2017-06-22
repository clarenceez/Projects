<template>
	<div id="app">
		<div class="layout">
			<div class="layout-ceiling"> 
				<div class="layout-logo">畦溪源F2FMIS</div>
				<div class="layout-ceiling-main">					
				<span style="color:white;margin-right:20px;display: inline-block;">欢迎：{{userName}}</span><a href="#" v-on:click="Logout">注销</a> 
				</div>
			</div>
			<div class="layout-center">
				<Row style="height: 100%;">
					<Col span="4" style="height:100%;border-right:0.5px #b2b2b2 solid">
					<Menu :theme=menu.Theme width="auto">
						<Submenu name=menu.Menu.MenuName>
							<template slot="title">
								<Icon :type='menu.Menu.Icon'></Icon>
								{{menu.Menu.MenuName}}
							</template>
							<Menu-item v-for="(m,index) in menu.Menu.SubMenu" :name="m.Name" key="m">
								<Icon :type='m.Icon'></Icon>
								<router-link :to='m.Url'>
									{{m.Name}}
								</router-link>
							</Menu-item>
						</Submenu>
					</Menu>
					</Col>
					<Col span="20">					
					<router-view >
						
					</router-view>
					</Col>
				</Row>
			</div>

		</div>
	</div>
</template>

<script>
	import menu from '@/json/menu'
	export default {
		name: 'app',
		data() {	
			var username=sessionStorage.UserName;			
			return {
				menu: menu,
				userName:username
			}
		},
		methods:{
			Logout(){				
				this.$store.commit('Logout')
			}
		}
	}
</script>

<style>
	html,
	body {
		height: 100%;
	}
	
	#app {
		font-family: 'Avenir', Helvetica, Arial, sans-serif;
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;
		text-align: center;
		color: #2c3e50;
		height: 100%;
	}
	
	.layout {
		border: 1px solid #d7dde4;
		background: #f5f7f9;
		position: relative;
		border-radius: 4px;
		overflow: atuo;
		height: 100%;
	}
	
	.layout-logo {
		width: auto;
		height: 30px;		
		border-radius: 3px;
		color:white;
		font-size: 20px;
		float: left;
		position: relative;		
		top: -5px;
		left: 40px;
	}
	
	.layout-header {
		height: 60px;
		background: #fff;
		box-shadow: 0 1px 1px rgba(0, 0, 0, .1);
	}
	
	.layout-center {
		height: 90%;		
	}
	
	.layout-copy {
		text-align: center;
		padding: 10px 0 20px;
		color: #9ea7b4;
		height: 40px;
		border-top: 1px solid #d7dde4;
		position: absolute;
		bottom: 0;
		width: 100%;
	}
	
	.layout-ceiling {
		background: #464c5b;
		padding: 18px 0;
		overflow: hidden;
		height: 56px;
	}
	
	.layout-ceiling-main {
		float: right;
		margin-right: 15px;
	}
	
	.layout-ceiling-main a {
		color: #9ba7b5;
	}
</style>