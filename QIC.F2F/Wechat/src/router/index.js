import Vue from 'vue'
import Router from 'vue-router'
import BindingPhone from '@/components/BindingPhone'
import UserInfo from '@/components/UserInfo'
import Hello from '@/components/Hello'
import DistributionList from '@/components/DistributionList'
import Vuex from 'vuex'
Vue.use(Router)

export default new Router({
	routes: [{
		name: 'BindingPhone',
		path: '/BindingPhone',
		component: BindingPhone
	}, {
		name: 'UserInfo',
		path: '/UserInfo',
		component: UserInfo
	}, {
		name: 'Hello',
		path: '',
		component: Hello
	}, {
		name: 'DistributionList',
		path: '/DistributionList',
		component: DistributionList
	}]
})