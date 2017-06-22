// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import Vuex from 'vuex'
import App from './App'
import router from './router'
import VueResource from 'vue-resource'
import Ajax from '@/js/Ajax.js'
import urlConfig from '@/json/urlConfig.json'
import VueScroller from 'vue-scroller'
Vue.config.productionTip = false
Vue.use(Vuex)
Vue.use(VueResource)
Vue.use(Ajax)
Vue.use(VueScroller)
const vuex_store = new Vuex.Store({
		state: {
			CaptchaTime: 60,
			urlConfigs: urlConfig,			
			IsDev: true, //是否为调试状态,发布为false
			Area:'http://192.168.0.143:8112/',//'http://m.creekfarm.cn/WebApi',  //完整域名
			ApiUrl: '/Api/Wechat', //webApi地址WebApi
			Headers: {
				'Content-Type': 'application/x-www-form-urlencoded',
				'Access-Control-Allow-Origin': '*',
				'QIC': 'QIC',
				"Authorization": ''
			} //请求头			

		},
		mutations: {
            UpdateHeader(state){
            	state.Headers.Authorization=sessionStorage["Token"];
            }
		}
	})
	/* eslint-disable no-new */
new Vue({
	el: '#app',
	router,
	store: vuex_store,
	created() {		
	}
})