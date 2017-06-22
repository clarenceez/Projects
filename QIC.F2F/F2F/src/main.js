// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import iView from 'iview'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import Login from '@/components/Login'
import 'iview/dist/styles/iview.css'
//import VueQuillEditor from 'vue-quill-editor'
import Ajax from '@/js/Ajax.js'
import urlConfig from '@/json/urlConfig.json'
Vue.config.productionTip = false
Vue.use(iView)
Vue.use(Vuex)
Vue.use(VueResource)
Vue.use(Ajax)
//Vue.use(VueQuillEditor)
	/* eslint-disable no-new */
router.beforeEach((to, from, next) => {
	iView.LoadingBar.start();
	next();
});

router.afterEach((to, from, next) => {
	iView.LoadingBar.finish();
});
const vuex_store = new Vuex.Store({
	state: {
		urlConfigs: urlConfig,
		IsDev: false, //是否为调试状态
		Area:'http://bm.creekfarm.cn:8010/WebApi', //'http://192.168.0.143:8112', //完整域名
		ApiUrl: '/WebApi/Api/Background', //webApi地址
		Headers: {
			'Content-Type': 'application/x-www-form-urlencoded',
			'Access-Control-Allow-Origin': '*'
		}, //请求头
		LoginState: false, //登录状态
		UserInfo: {
			UserName: ''
		}
	},
	mutations: {
		Login(state, payload) {
			state.LoginState = true;
			state.UserInfo = {
				UserName: payload.user
			};
			sessionStorage.LoginState = state.LoginState;
			sessionStorage.UserName = state.UserInfo.UserName;
		},
		Logout(state) {
			state.LoginState = false;
			sessionStorage.LoginState = state.LoginState;
		}
	}
})
new Vue({
	el: '#app',
	router,
	store: vuex_store,
	watch: {
		"$route": "CheckLogin"
	},
	methods: {
		GetLoginState() {
			if(sessionStorage.LoginState !== undefined)
				this.$store.state.LoginState = (/^true$/i).test(sessionStorage.LoginState);
			return this.$store.state.LoginState;
		},
		GetUserInfo() {
			this.$store.state.UserInfo.UserName = sessionStorage.UserName;
			return this.$store.state.UserInfo;
		},
		CheckLogin: function() {
			if(!this.GetLoginState()) {
				router.push({
					path: '/'
				});
			}
		},
		Logout: function() {
			this.$store.commit('Logout')
		}
	},
	created: function() {	
		if(!this.GetLoginState()) {
			router.push({
				path: '/'
			})
		}
	}
})