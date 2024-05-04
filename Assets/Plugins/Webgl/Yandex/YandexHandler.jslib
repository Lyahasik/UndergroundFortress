mergeInto(LibraryManager.library, {

    ShowAdsInterstitialExtern: function () {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function(wasShown) {
                    myGameInstance.SendMessage('PublishHandler', 'EndAds');
                },
                onError: function(error) {
                    myGameInstance.SendMessage('PublishHandler', 'EndAds');
                },
                onOffline: function() {
                    myGameInstance.SendMessage('PublishHandler', 'EndAds');
                }
            }
        })
    },

    ShowAdsRewardExtern: function () {
        ysdk.adv.showRewardedVideo({
            callbacks: {
                onRewarded: () => {
                    myGameInstance.SendMessage('PublishHandler', 'ClaimRewardAds');
                },
                onClose: () => {
                    myGameInstance.SendMessage('PublishHandler', 'EndAds');  
                }
            }
        })
    },
     
     CheckPurchasesExtern: function () {
         if (!payments)
             return;
             
         payments
         .getPurchases()
         .then(purchases => purchases.forEach((purchase) => {
             console.log('Подтверждение покупки ' + purchase.productID);
             myGameInstance.SendMessage('PublishHandler', 'ClaimRewardPurchaseId', purchase.productID);
             payments.consumePurchase(purchase.purchaseToken)}))
         .catch(err => { console.log('check goods exception') })
     },
 
     BuyPurchaseExtern: function (idPurchase) {
         if (!payments)
             return;
 
         var idString = UTF8ToString(idPurchase);
 
         payments
         .purchase({ id: idString })
         .then(purchase => {
             myGameInstance.SendMessage('PublishHandler', 'ClaimRewardPurchase');
             payments.consumePurchase(purchase.purchaseToken);
             })
         .catch(err => { console.log('buy goods exception') })
     },

    LoadDataExtern: function () {
        if (player && player.getMode() !== 'lite') {
            player.getData().then(_data => {
            const json = JSON.stringify(_data);
            console.log(json);
            myGameInstance.SendMessage('PublishHandler', 'LoadProgress', json);
        })
        } else {
            myGameInstance.SendMessage('PublishHandler', 'LoadProgress', 'LocalProgress');
        }
        
    },

    SaveDataExtern: function (data) {
        var dataString = UTF8ToString(data);
        var json = JSON.parse(dataString);
        console.log(json);
        player.setData(json);
    },

    CheckRateGameExtern: function () {
        ysdk.feedback.canReview()
        .then(({ value, reason }) => {
            if (value) {
                myGameInstance.SendMessage('PublishHandler', 'StartRateGame');
            }
        })
    },

    RateGameExtern: function () {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
            console.log(feedbackSent);
        })
    },
     
    SetLeaderBoardExtern: function (value) {
        if (!ysdk || !player || player.getMode() === 'lite')
            return;
    
        try {
            ysdk
            .getLeaderboards()
            .then(lb => {
                lb.setLeaderboardScore('Leaders', value);
            })
        } catch (err) { console.log('set leaderboard exception') }
    }
});