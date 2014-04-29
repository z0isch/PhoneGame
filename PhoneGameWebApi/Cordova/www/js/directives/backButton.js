(function () {
  'use strict';
  angular.module('phonegame').directive('backButton', ['backButton',function (backButton) {
    function link(scope, element, attrs) {

      if (scope.handler)
        backButton.setHandler(scope.handler);
      else
        backButton.useDefaultHandler();

      scope.backButtonPress = function () {
        backButton.triggerHandler();
      };
      
      scope.$on('$destroy', function () {
        backButton.useDefaultHandler();
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