(function () {
  'use strict';
  angular.module('app').controller('PickPhraseCtrl', ['$scope', '$ionicPlatform', '$http',
    '$state', '$stateParams', '$ionicLoading', '$ionicNavBarDelegate', 'phraseService','gameService',
    function ($scope, $ionicPlatform, $http, $state, $stateParams, $ionicLoading, $ionicNavBarDelegate, phraseService, gameService) {
      $scope.backButtonHandler = function () {
        $state.go('main');
      };

      $scope.phrasePicked = function (phrase) {
        var loadingScreen = $ionicLoading.show({
          content: 'Choosing Phrase...',
        });
        var promise = phraseService.pickPhrase($stateParams.gameId, phrase.id).then(function (data) {
          $state.go('game', { gameId: data.ID });
        }, function (error) {
          alert(error);
        });
        promise['finally'](function () {
          loadingScreen.hide();
        });
      }
      var loadingScreen = $ionicLoading.show({
        content: 'Getting Phrases...',
      });
      var promise = phraseService.getPhrases().then(function (data) {
        $scope.phrases = data;
      }, function (error) {
        alert(error);
      });
      promise['finally'](function () {
        loadingScreen.hide();
      });
    }
  ])
})();