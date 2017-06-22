import Vue from 'vue'
import Router from 'vue-router'
import Hello from '@/components/Hello'
import FamlilyManagement from '@/components/FamlilyManagement'
import Distribution from '@/components/Distribution'
import Card from '@/components/Card'
import Product from '@/components/Product'
import FamilyCard from '@/components/FamilyCard'
import ProductReport from '@/components/ProductReport'
import Login from '@/components/Login'
import App from '@/App'
Vue.use(Router)

export default new Router({
	//	mode: 'history',
	routes: [{
		path: '',
		component: Login
	}, {
		path: '/Home',
		component: App,
		children: [{
			path: 'FamlilyManagement',
			component: FamlilyManagement
		}, {
			path: 'Distribution',
			component: Distribution
		}, {
			path: 'Card',
			component: Card
		}, {
			path: 'Product',
			component: Product
		},{
			path: 'FamilyCard',
			component: FamilyCard
		},{
			path: 'ProductReport',
			component: ProductReport
		}, {
			path: '',
			component: Hello
		}]
	}]
})