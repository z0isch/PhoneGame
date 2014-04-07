(function () {
  angular.module('app', ['ionic', 'oauth','phoneGame'])
    .config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
      .state('login', {
        url: '/login',
        templateUrl: 'login.html',
        controller: 'LoginCtrl'
      })
      .state('main', {
        url: '/',
        templateUrl: 'main.html',
        controller: 'MainCtrl'
      });

      $urlRouterProvider.otherwise("/");

    })
    .controller('LoginCtrl', ['$scope', '$ionicPlatform', '$http', 'authenticationService', '$state', '$ionicLoading',
      function ($scope, $ionicPlatform, $http, authenticationService, $state, $ionicLoading) {
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
    .controller('MainCtrl', ['$scope', '$ionicPlatform', '$http', '$state', 'authenticationService', 'playersService', '$ionicLoading',
      function ($scope, $ionicPlatform, $http, $state, authenticationService, playersService, $ionicLoading) {
        $scope.loggedIn = false;

        $scope.goToLogin = function () {
          $state.go('login');
        }
        $scope.logOut = function () {
          authenticationService.logUserOut();
          $scope.loggedIn = false;
          $scope.$apply();
        }

        $ionicPlatform.ready(function () {
          if (authenticationService.isLoggedIn()) {
            var credentials = authenticationService.getCredentials();
            $scope.loggedIn = true;
            $scope.$apply();

            var loadingScreen = $ionicLoading.show({
              content: 'Getting account info...',
            });

            var promise = playersService.getPlayer(credentials.phone_game_id);
            promise.then(function (player) {
              $scope.name = player.Name;
            }, function (error) {
              alert(error);
            });

            promise['finally'](function () {
              loadingScreen.hide();
            });
          }
        });
      }
    ]);
})();
