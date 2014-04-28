(function () {
  'use strict';
  angular.module('app').controller('PickPhraseCtrl', ['$scope', '$ionicPlatform', '$http',
    '$state', '$stateParams', '$ionicLoading', '$ionicNavBarDelegate', 'phraseService','backButton','gameService',
    function ($scope, $ionicPlatform, $http, $state, $stateParams, $ionicLoading, $ionicNavBarDelegate, phraseService, backButton, gameService) {
      $scope.backButtonHandler = function () {
        var loadingScreen = $ionicLoading.show({
          content: '',
        });
        var promise =  gameService.deleteGame($stateParams.gameId).then(function (data) {
          $state.go('main');
        }, function (error) {
          alert(error);
        });
        promise['finally'](function () {
          loadingScreen.hide();
        });
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