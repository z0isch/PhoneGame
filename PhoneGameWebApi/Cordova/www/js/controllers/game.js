(function () {
  'use strict';
  angular.module('app')
  .controller('GameCtrl', ['$scope', '$ionicPlatform', '$http',
      '$state', '$stateParams', '$ionicLoading', '$ionicNavBarDelegate', 'gameService',
      function ($scope, $ionicPlatform, $http, $state, $stateParams, $ionicLoading, $ionicNavBarDelegate, gameService) {
        var loadingScreen = $ionicLoading.show({
          content: 'Getting Game...',
        });
        var promise = gameService.getGame($stateParams.gameId).then(function (data) {
          $scope.game = data;
        }, function (error) {
          alert(error);
        });
        promise['finally'](function () {
          loadingScreen.hide();
        });
      }
  ]);
})();