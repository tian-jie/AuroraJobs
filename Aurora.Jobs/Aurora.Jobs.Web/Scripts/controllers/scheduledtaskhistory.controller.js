//======================================================================
// Copyright (c) Aurora.Jobs  development team. All rights reserved.
// 所属项目：Aurora.Jobs js.controller
// 创 建 人：Aurora.Jobs development team
// 创建日期：2017-07-06 16:21:57
// 用    途：后台任务 (ScheduledTask)js控制器
//======================================================================

/**
*加载后台任务列表
**/
function ScheduledTaskHistoryListController($scope, $http, $window) {
    var gridId = 'data_grid';
    $scope.LoadScheduledTaskHistoryList = function () {
        oUtils.jqGrid({
            gridId: gridId,
            url: '/ScheduledTaskHistory/List?r=' + Math.random(),
            dataIdKey: 'ScheduledTaskHistoryId',
            isPager: true,
            colNames: [
                         "操作",
                        "记录ID",
                        "JobId",
                        "名称",
                        "执行时间",
                        "执行持续时长",
                        "创建日期时间",
                        "日志内容",
            ],
            colModel: [
                        { name: 'ScheduledTaskHistoryId', index: 'ScheduledTaskHistoryId', width: 75, formatter: $scope.formatModel, edittype: 'custom' },
                        { name: "ScheduledTaskHistoryId", width: 60, index: "ScheduledTaskHistoryId", sortable: false, hidden: true },
                        { name: "ScheduledTaskId", width: 60, index: "ScheduledTaskId", sortable: false, hidden: true },
                        { name: "JobName", width: 150, index: "JobName", sortable: false },
                        { name: "ExecutionTime", width: 145, index: "ExecutionTime", sortable: false },
                        { name: "ExecutionDuration", width: 80, index: "ExecutionDuration", sortable: false },
                        { name: "CreatedDateTime", width: GridCreatedDateTimeWidth, index: "CreatedDateTime", sortable: false },
                        { name: "RunLog", width: 250, index: "RunLog", sortable: false },
            ]
        });
    };

    $scope.formatModel = function (cellValue, options, rowObject) {
        var btns = [];
        btns.push('<a href="javascript:void(0);" onclick="ShowEditFrame(\'' + cellValue + '\')" class="btn btn-info btn-xs" >详情</a>');
        return btns.join('&nbsp;&nbsp;&nbsp;&nbsp;');
    };

    $scope.model = {
        JobName: '',
    };

    $(function () {
        $scope.LoadScheduledTaskHistoryList();

        $(".query-container .btn-search").click(function () {
            var parameter = "";
            parameter += 'JobName:' + "'" + escape($scope.model.JobName) + "'";
            oUtils.jqGrid.search(gridId, parameter);
        });
    });
};


/**
*后台任务修改视图控制器
**/
function ScheduledTaskHistoryInfoController($scope, $http, $window) {

    var request = oUtils.urlParameter();
    var ScheduledTaskHistoryId = request["ScheduledTaskHistoryId"];

    $scope.model = {
        ScheduledTaskHistoryId: ScheduledTaskHistoryId,
        ScheduledTaskId: '',
        Name: '',
        ExecutionTime: '',
        ExecutionDuration: '',
        RunLog: '',
    };

    $scope.LoadScheduledTaskHistoryInfo = function () {
        $scope.model.ScheduledTaskHistoryId = ScheduledTaskHistoryId;
        $http({
            method: 'GET',
            url: '/ScheduledTaskHistory/InfoData?r=' + Math.random(),
            cache: false,
            params: { ScheduledTaskHistoryId: ScheduledTaskHistoryId }
        })
       .success(function (result, status, headers, config) {
           if (result.success) {
               $scope.model = result.data;
           } else {
               oUtils.alertError(result.message);
           }
       })
       .error(function (result, status, headers, config) {
           alert(result.message || '请求过程中发生异常');
       });
    };


    $(function () {
        $scope.LoadScheduledTaskHistoryInfo();
    });
};