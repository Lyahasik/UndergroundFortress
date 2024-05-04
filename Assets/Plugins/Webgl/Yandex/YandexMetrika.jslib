mergeInto(LibraryManager.library, {

    LoadedExtern: function () {
        ym(97174711,'reachGoal','loaded');
    },

    TargetAdsExtern: function (id) {
        ym(97174711,'reachGoal','ads' + id);
    },

    TargetPurchasesExtern: function (id) {
        ym(97174711,'reachGoal','purchase' + id);
    },

    TargetLevelsExtern: function (number) {
        ym(97174711,'reachGoal','level' + number);
    },

    TargetTutorialExtern: function (number) {
        ym(97174711,'reachGoal','tutorial' + number);
    },

    TargetActivityExtern: function (number) {
        ym(97174711,'reachGoal','activity' + number);
    }
});