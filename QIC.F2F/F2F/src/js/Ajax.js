exports.install = function(Vue, options) {
	Vue.prototype.ajax = function(options) {
		var options = options || {};
		var type = options.type || 'get';
		var headers = this.$store.state.Headers || {};
		options.headers = headers;
		var url = options.url || '';
		if(this.$store.state.IsDev){
			url=`${this.$store.state.Area}${this.$store.state.ApiUrl}${url}`;
		}
		else{
			url=`${this.$store.state.ApiUrl}${url}`;
		}
		var success = options.success || function(data) {};
		var error = options.error||function(){};
		switch(type.toLowerCase()) {
			case 'get':
				{
					this.$http.get(url, options).then(success, error);
				};
				break;
			case 'post':
				{		
//					options.emulateJSON=true;
					this.$http.post(url,options.params, {headers:headers,emulateJSON:true}).then(success, error);
				};
				break;
		}
	};
};