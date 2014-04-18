(function () {
	angular.module('cordova.contacts', [])
	.service('contacts', ['$q', '$timeout',
		function ($q, $timeout) {
			this.find = function (options) {
				var deferred = $q.defer();
				deferred.resolve([{
				  id: '1',
				  displayName: 'Tester2',
				  phoneNumbers: [{
				    type: 'Cell',
				    value: 'TEST2',
				    pref: true
				  }]
				}, {
				  id: '2',
				  displayName: 'Tester3',
				  phoneNumbers: [{
				    type: 'Cell',
				    value: 'TEST3',
				    pref: true
				  }]
				}
				]);
				return deferred.promise;
			};
		},
	])
})();