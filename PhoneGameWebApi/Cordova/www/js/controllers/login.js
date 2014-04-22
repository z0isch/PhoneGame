(function () {
  'use strict';
  angular.module('app').controller('LoginCtrl', ['$scope', '$ionicPlatform', '$http', 'authenticationService', '$state', '$ionicLoading', '$ionicNavBarDelegate',
    function ($scope, $ionicPlatform, $http, authenticationService, $state, $ionicLoading, $ionicNavBarDelegate) {
      $ionicPlatform.ready(function () {
        $scope.googleLogin = function () {
          var promise = authenticationService.openOAuthWindow("google");
          var loadingScreen = $ionicLoading.show({
            content: 'Logging in...',
          });
          promise.then(function (credentials) {
            authenticationService.saveCredentials(credentials);
            $state.go('main');
          }, function (error) {
            alert(error);
          });

          promise['finally'](function () {
            loadingScreen.hide();
          });

        }
      });
    }
  ])
})();