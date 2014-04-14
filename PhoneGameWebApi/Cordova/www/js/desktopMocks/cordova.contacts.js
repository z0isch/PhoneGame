(function () {
	angular.module('cordova.contacts', [''])
	.service('contacts', ['$q', '$timeout',
		function ($q, $timeout) {
			this.find = function (options) {
				var deferred = $q.defer();

				deferred.resolve([{
					id:'1',
					displayName:'Tester2',
					phoneNumbers:[{
						type:'Cell',
						value:'Test2',
						pref:true
					}]
				}]);

				return deferred.promise;
			};
		}
	])
})();