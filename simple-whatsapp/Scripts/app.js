var chat = $.connection.messanger;

var app = angular.module("myApp", []);

app.controller('AgProgramlamaCtrl', ['$scope', '$http', function ($scope, $http) {


    $scope.sessionId = model.sessionId;
    $scope.sessionMail = model.sessionMail;

    $scope.Users = model.users;
    $scope.Groups = model.groups;

    $scope.Model = null;

    $scope.ilkAcilis = false;

    $scope.KonusulanKisi;


    $scope.GonderilecekYerSetEt = function () {

        $scope.Type = "herkeze";
    }


    chat.client.Hello = function () {

        alert("s.a");

    }
    chat.client.PrivateMessageArrived = function (messageObject) {

        if ($scope.GonderilecekYer == messageObject.UserId) {
            $scope.Model.messages.push(messageObject);
            $scope.$apply();

            chat.server.messageSeen(model.sessionId, messageObject.UserId);


            var chatScroll = $("#conversation");
            var scrollPosition = chatScroll.scrollTop();
            var scrollTo_int = chatScroll.prop('scrollHeight');
            var sonuc = scrollTo_int - scrollPosition;
            if (sonuc <= 898) {
                $("#conversation").scrollTop(scrollTo_int);
            }

        }
        else {

            $.notify(messageObject.UserMail + " kullanıcısından mesaj var", "success");
            $scope.messageSeenCountIncrement(messageObject.UserId)

        }


    }
    chat.client.SendMessageSuccess = function (messageObject, UserId) {
        if ($scope.GonderilecekYer == UserId) {
            $scope.Model.messages.push(messageObject);
            $scope.$apply();


            var chatScroll = $("#conversation");
            var scrollPosition = chatScroll.scrollTop();
            var scrollTo_int = chatScroll.prop('scrollHeight');
            var sonuc = scrollTo_int - scrollPosition;
            if (sonuc <= 898) {
                $("#conversation").scrollTop(scrollTo_int);
            }

        }
        else {
            //Şuan bişey yapması gerekmiyor
        }

    }
    chat.client.GroupMessageArrived = function (messageObject, receiverId,groupName) {

        if ($scope.GonderilecekYer == receiverId) {
            $scope.Model.messages.push(messageObject);
            $scope.$apply();


            var chatScroll = $("#conversation");
            var scrollPosition = chatScroll.scrollTop();
            var scrollTo_int = chatScroll.prop('scrollHeight');
            var sonuc = scrollTo_int - scrollPosition;
            if (sonuc <= 898) {
                $("#conversation").scrollTop(scrollTo_int);
            }

        }
        else {
            $.notify(messageObject.UserMail + " kullanıcısından" + groupName + " grubuna mesaj var", "success");
        }


    }


    $.connection.hub.start().done(function () {

        chat.server.onlineOl(model.sessionId);

        $scope.showMessagesUser = function (user) {

            $scope.Konusulan = user.email;
            $scope.GonderilecekYer = user.id;
            $scope.Type = "user";

            $.ajax({
                url: "/Home/GetPrivateMessages",
                type: "POST",
                data: { userId: user.id },
            }).done(function (d) {

                $scope.ilkAcilis = true;
                $scope.Model = d;
                console.log($scope.Model);
                $scope.$apply();

                $scope.messageSeenCountReset(user.id);

                chat.server.messageSeen($scope.sessionId, user.id);

                var chatScroll = $("#conversation");
                var scrollTo_int = chatScroll.prop('scrollHeight');
                $("#conversation").scrollTop(scrollTo_int);
                $("#message").focus();
            });

        }
        $scope.showMessagesGroup = function (group) {

            if ($scope.GonderilecekYer != null) {
                chat.server.leaveGroup($scope.GonderilecekYer);
            }

            $scope.Konusulan = group.groupName;
            $scope.GonderilecekYer = group.id;
            $scope.Type = "group";



            $.ajax({
                url: "/Home/GetGroupMessages",
                type: "POST",
                data: { groupId: group.id },
            }).done(function (d) {

                $scope.ilkAcilis = true;
                $scope.Model = d;
                $scope.$apply();

                chat.server.joinGroup($scope.GonderilecekYer);

                console.log($scope.Model);


                var chatScroll = $("#conversation");
                var scrollTo_int = chatScroll.prop('scrollHeight');
                $("#conversation").scrollTop(scrollTo_int);
                $("#message").focus();
            });


        }
        $scope.messageSend = function () {

            var message = $("#message").val();

            if ($scope.Type == "user") {

                if (message != "") {
                    chat.server.sendPrivateMessage(model.sessionId, $scope.GonderilecekYer, message);
                    $("#message").val("")
                    $("#message").focus();
                }
            }
            else if ($scope.Type == "group") {
                if (message != "") {
                    chat.server.sendGroupMessage(model.sessionId, $scope.GonderilecekYer, message);
                    $("#message").val("")
                    $("#message").focus();
                }

            } else {

                if ($("#broadcastMessage").val() != "") {
                    chat.server.sendBroadcastMessage(model.sessionId, $("#broadcastMessage").val());
                    $("#broadcastMessage").val("");
                    $("#broadcastMessage").focus();
                }
            }


        }
        $('#message').keypress(function (e) {
            var key = e.which;
            if (key == 13)  // the enter key code
            {
                $scope.messageSend();
                if (event.preventDefault) event.preventDefault(); // This should fix it
                return false; // Just a workaround for old browsers
            }
        });


        $scope.messageSeenCountReset = function (userId) {

            for (var i = 0; i < $scope.Users.length; i++) {

                if ($scope.Users[i].id == userId) {
                    $scope.Users[i].gorummemisMesajSayisi = 0;
                    $scope.$apply();
                }

            }
        }
        $scope.messageSeenCountIncrement = function (userId) {

            for (var i = 0; i < $scope.Users.length; i++) {

                if ($scope.Users[i].id == userId) {
                    $scope.Users[i].gorummemisMesajSayisi++;
                    $scope.$apply();
                }

            }
        }


    }).fail(function () {

    });

}]);





app.filter("mydate", function () {
    var re = /\/Date\(([0-9]*)\)\//;
    return function (x) {
        var m = x.match(re);
        if (m) return new Date(parseInt(m[1]));
        else return x;
    };
});