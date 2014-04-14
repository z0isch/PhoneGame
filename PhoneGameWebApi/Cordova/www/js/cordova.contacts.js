(function () {
  angular.module('cordova.contacts', [])
	.service('contacts', ['$q',
		function ($q) {
		  this.find = function () {
		    var deferred = $q.defer();

		    var options = new ContactFindOptions();
		    options.filter = "";
		    options.multiple = true;

		    navigator.contacts.find(["id", "displayName", "phoneNumbers"], function (contacts) {
		      deferred.resolve(contacts);
		    },
        function (error) {

          deferred.reject("Failed to get contacts: "+JSON.stringify(error));
        }, options);

		    return deferred.promise;
		  };
		}
	])
})();