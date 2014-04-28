(function () {
  'use strict';
  angular.module('phonegame').directive('backButton',['backButton',function (backButton) {
    function link(scope, element, attrs) {
      scope.backButtonPress = function () {
        document.dispatchEvent(new Event('backbutton'));
      };
      backButton.setHandler(function () {
        if (scope.handler)
          scope.handler();
      });
    }
    return {
      restrict: 'E',
      templateUrl: 'js/directives/back-button.html',
      link: link,
      scope: {
        handler: "=handler"
      }
    };
  }]);
})();