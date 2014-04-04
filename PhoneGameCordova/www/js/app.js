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
    .controller('LoginCtrl', ['$scope', '$ionicPlatform', '$http', 'authentication', '$state', '$ionicLoading',
      function ($scope, $ionicPlatform, $http, authentication, $state, $ionicLoading) {
        $ionicPlatform.ready(function () {
          $scope.googleLogin = function () {
            var promise = authentication.openOAuthWindow("google");
            var loadingScreen = $ionicLoading.show({
              content: 'Logging in...',
            });
            promise.then(function (credentials) {
              authentication.saveCredentials(credentials);
              $state.go('main');
              loadingScreen.hide();
            }, function (error) {
              alert(error);
              loadingScreen.hide();
            }, function (notification) {});
          }
        });
      }
    ])
    .controller('MainCtrl', ['$scope', '$ionicPlatform', '$http', '$state', 'authentication', 'playersService', '$ionicLoading',
      function ($scope, $ionicPlatform, $http, $state, authentication, playersService, $ionicLoading) {
        $scope.loggedIn = false;

        $scope.goToLogin = function () {
          $state.go('login');
        }
        $scope.logOut = function () {
          authentication.logUserOut();
          $scope.loggedIn = false;
          $scope.$apply();
        }

        $ionicPlatform.ready(function () {
          if (authentication.isLoggedIn()) {
            var credentials = authentication.getCredentials();
            $scope.loggedIn = true;
            $scope.$apply();

            var loadingScreen = $ionicLoading.show({
              content: 'Getting account info...',
            });

            var promise = playersService.getPlayer(credentials.phone_game_id);
            promise.then(function (player) {
              $scope.name = player.Name;
              loadingScreen.hide();
            }, function (error) {
              alert(error);
              loadingScreen.hide();
            },
            function (notifications) { });
          }
        });
      }
    ]);
})();
