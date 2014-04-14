(function () {
  angular.module('cordova.contacts', [''])
	.service('contacts', ['$q', '$timeout',
		function ($q, $timeout) {
		  this.find = function (options) {
		    var deferred = $q.defer();

		    navigator.contacts.find(fields, function (contacts) {
		      deferred.resolve(contacts);
		    },
        function (error) {
          deferred.reject("Failed to get contacts");
        }, options);

		    return deferred.promise;
		  };
		}
	])
})();