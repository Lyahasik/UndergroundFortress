mergeInto(LibraryManager.library, {

    LoadedExtern: function () {
        ym(94529710,'reachGoal','Loaded');
    },

    StartGameExtern: function () {
        ym(94529710,'reachGoal','Start_game');
    },

    TimeStayExtern: function (target) {
        ym(94529710,'reachGoal','Time_stay_' + target);
    },

    LevelSuccessExtern: function (numberLevel) {
        ym(94529710,'reachGoal','Level_success_' + numberLevel);
    },

    ActiveAdsSpawnPointExtern: function () {
        ym(94529710,'reachGoal','Total_spawn_points');
    },

    ActiveAdsSpawnPointsExtern: function (numberBowls) {
        ym(94529710,'reachGoal','Spawn_point_' + numberBowls);
    },

    OpenAdsLevelExtern: function () {
        ym(94529710,'reachGoal','Total_ads_levels');
    },
     
    OpenAdsLevelsExtern: function (numberLevels) {
        ym(94529710,'reachGoal','Ads_level_' + numberLevels);
    }
});