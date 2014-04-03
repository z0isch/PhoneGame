(function () {
  angular.module('game', [])
  .service('playersService', ['$http', '$q', function ($http, $q) {
    this.getPlayer = function (credentials, id) {
      var deferred = $q.defer();
      $http({
        method: 'GET', url: 'http://54.200.69.198/phonegameservice/api/Players/' + id, headers: {
          'oauth_encrypted_token': credentials.oauth_encrypted_token,
          'oauth_provider': credentials.oauth_provider,
          'phone_game_id': credentials.phone_game_id
        }
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