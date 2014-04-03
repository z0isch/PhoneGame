(function () {
  angular.module('phoneGame', ['oauth'])
  .service('playersService', ['$http', '$q', 'authentication', function ($http, $q, authentication) {
    this.getPlayer = function (id) {
      var deferred = $q.defer();
      $http({
        method: 'GET',
        url: 'http://54.200.69.198/phonegameservice/api/Players/' + id,
        headers: authentication.getOAuthHeaders()
      })
       .success(function (data, status, headers, config) {
         deferred.resolve(data);
       })
      .error(function (data, status, headers, config) {
        if(status == "404")
          deferred.reject("User not logged in");
        else if(status == "400")
          deferred.reject("User not found");
      })
      return deferred.promise;
    }
  }]);
})();