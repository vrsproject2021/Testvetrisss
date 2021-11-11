parent.HideLoad();

var selectedTz = null;
var selectedTimezone = null;
var editingTimezone = null;
var editingTz = null;
var tzs = null;
var selectedReader = null;
var selectedGroup = null;
var mode = "group";
var suppress = false;
// date range store
var range = [new Date(), new Date(new Date() + 7 * 24 * 3600 * 1000)];
var date1, date2;
// readers store
var readers = [];
var userDefaults={};
// readers group store
var readerGroups = [];

// timezones store
var tz = [];

// new/edit schedule data object
var scheduleEditData = null;

// default date format from setting compatible to Flatpicker
// if setting is 'dd-MM-yyyy' it will be 'd-m-Y'
// if setting is 'MM/dd/yyyy' it will be 'm/d/Y'

var dateFormat = parent.GsDateFormat.replace(/y{1,}/, 'Y').replace(/M{1,}/, 'm').replace(/d{1,}/, 'd')
var ldateFormat = "l "+parent.GsDateFormat.replace(/y{1,}/, 'Y').replace(/M{1,}/, 'm').replace(/d{1,}/, 'd')
var momentDateFormat = parent.GsDateFormat.replace(/y{1,}/, 'YYYY').replace(/M{1,}/, 'MM').replace(/d{1,}/, 'DD')

// show loading status
function PopupProcess() {
    return parent.PopupProcess();
}
// hide loading status
function HideProcess() {
    return parent.HideProcess();
}

function btnClose_OnClick() {

    parent.GsRetStatus = "false";
    parent.PopupLoad();
    parent.LoadHome();
}


// timezone on selection event handler
$("#tz").on('select2:select', function(e) {
    if (e.params.data.id == "") {
        selectedTz = null;
        selectedTimezone = null;
    } else {
        selectedTz = e.params.data.offset;
        selectedTimezone = e.params.data.text;
    }
    $(".apply").trigger("click");
});

// group/reader option on selection event handler
$("#option").on("change", function(e) {

    mode = e.target.value;
    if (mode == "group") {
        $("#groups").css("display", "");
        $("#readers").css("display", "none");
        selectedReader = null;
        Schedule.setReaders(readers);
        $("#reader").val(selectedReader).trigger('change');
        //if (selectedGroup == null) {
        //    selectedGroup = readerGroups[0].id;
        //}
        //selectedGroup = null;
        $("#group").val(selectedGroup).trigger('change');
        if (selectedGroup !== null)
            Schedule.setReaders(readers.filter(function(i) { return i.groupId == selectedGroup; }));


    } else {
        selectedGroup = null;
        $("#group").val(selectedGroup).trigger('change');
        $("#groups").css("display", "none");
        $("#readers").css("display", "");
        if (selectedReader == null) {
            selectedReader = readers[0].id;
            $("#reader").val(selectedReader).trigger('change');
        }
        Schedule.setReaders(readers.filter(i => i.id == selectedReader));
    }
    $(".apply").trigger("click");
});

// group on selection event handler
$("#group").on('select2:select', function(e) {
    if (e.params.data.id == "") {
        selectedGroup = null;
        Schedule.setReaders(readers);
        Schedule.setShowByGroupColor(true);
    } else {
        selectedGroup = e.params.data.id;
        Schedule.setReaders(readers.filter(function(i) { return i.groupId == selectedGroup; }));
        Schedule.setShowByGroupColor(false);
    }
    $(".apply").trigger("click");
});

// reader on selection event handler
$("#reader").on('select2:select', function(e) {
    selectedGroup = null;
    if (e.params.data.id == "") {
        selectedReader = null;
        Schedule.setReaders(readers);
    } else {
        selectedReader = e.params.data.id;
        Schedule.setReaders(readers.filter(function(i) { return i.id == selectedReader; }));
    }
    Schedule.setShowByGroupColor(false);
    $(".apply").trigger("click");
});

/*
 * Generate hours and mins items
 */
var hours = Array(12).fill().map((_, i) => `<option value="${(i==0?12:i)}">${((i==0?12:i)<10?"0":"")+(i==0?12:i)}</option>`).join("");
var mins = Array(60).fill().map((_, i) => `<option value="${i}">${(i<10?"0":"")+i}</option>`).join("");

$(`select[id^="hh"]`).each(function(i, e) {
    e.innerHTML = (hours);
})
$(`select[id^="mm"]`).each(function(i, e) {
    e.innerHTML = (mins);
});

/*
 * Initialize date controls from and to
 */
var fromDate, toDate;
fromDate = flatpickr("#fromDate", {
    defaultDate: range[0],
    dateFormat: dateFormat,

    onClose: function(selectedDates, dateStr, instance) {
        var days = parseInt($("#next").val()) - 1;
        var from = selectedDates[0];
        var to = moment(from).add(days, "days").toDate();
        range = [from, to];
        toDate.setDate(to);
        $(".apply").trigger("click");
    }
});
toDate = flatpickr("#toDate", {
    defaultDate: range[1],
    dateFormat: dateFormat,
    clickOpens: false
});


// days (next days from fromDate) on selection event handler
$("#next").on("change", function(e) {
    var days = parseInt(e.target.value) - 1;
    var from = range[0];
    var to = moment(from).add(days, "days").toDate();

    range = [from, to];
    toDate.setDate(to);
    $(".apply").trigger("click");
});

/*
 *  Set todate depending on start date and days (init) 
 */
(function() {
    var days = parseInt($("#next").val()) - 1;
    var from = range[0];
    var to = moment(from).add(days, "days").toDate();

    range = [from, to];
    toDate.setDate(to);
})();

/*
 *  Create button event handler
 *
 */
$("#create").on("click", function() {
    if(userDefaults.roleCode=="RDL"){
        return;
    }
    $("#scheduleTitle").text("New schedule");
    $("#rcolor").html("");

    setupDialog(function() {
        var defaultTz = tz.find(i => i.isDefault == true);
        var defaultTzName = defaultTz.text;
        $("#defaultTz").html(`<span>Conversion: '${defaultTzName}'</span>`);

        scheduleEditData = {
            id: '00000000-0000-0000-0000-000000000000',
            readerId: null,
            date: null,
            startTime: null,
            endTime: null,
            notes: '',
            userId: parent.objhdnUserID.value,
            menuId: 0, //parent.objhdnMenuID.value,
            extra: {
                date1: null,
                date2: null,
            }
        };

        var groupFilter = $("#option").val() == "group";
        var _readers = [];
        var readerId;
        $('.editmode').hide();
        $('.newmode').show();
        $('#deletebtn').hide();
        $('#notes').val("");
        $("#readerId").removeAttr("disabled")
        $('#readerId').val("").trigger("change.select2");

        if (groupFilter) {
            var readersrc = readers.map(function(i) { return { id: i.id, text: i.name, groupId: i.groupId }; });
            if (selectedGroup) readersrc = readersrc.filter(i => i.groupId == selectedGroup);
            $('#readerId').html('');
            $("#readerId").select2({
                data: readersrc
            });
            if (readersrc.length > 0) {
                selectedReader = null;
                readerId = readersrc[0].id;;
                $("#readerId").val(readerId).trigger('change');
                scheduleEditData.readerId = readerId;
            }
        } else {
            if (selectedReader) {
                readerId = selectedReader;
                $('#readerId').html('');
                $("#readerId").attr("disabled", "disabled");
                $("#readerId").select2({
                    data: readers.filter(i => i.id == selectedReader).map(function(i) { return { id: i.id, text: i.name, groupId: i.groupId }; })
                });
                $("#readerId").val(selectedReader).trigger('change');
                scheduleEditData.readerId = readerId;
                var r = readers.find(i => i.id == readerId);
                $("#rcolor").html(`<span style="height: 16px; width:16px; background-color:${r.color}; border-radius:50%;display:block;margin-bottom:4px;"></span>`);
            } else {
                $('#readerId').html('');
                ("#readerId").select2({
                    data: readers.map(function(i) { return { id: i.id, text: i.name, groupId: i.groupId }; })
                });
            }
        }



        $('#readerId').on('select2:select', function(e) {
            readerId = e.params.data.id;
            scheduleEditData.readerId = readerId;
            onChangeReader(scheduleEditData);
        });

        onChangeReader(scheduleEditData);

        $("#savebtn").bind("click", function(ev) {
            // save for new
            var h = parseInt($("#hh1").val());
            var m = parseInt($("#mm1").val());
            var pm = $("#ampm1").val() === "PM";
            if (h == 12) h -= 12;
            if (pm) h += 12;

            scheduleEditData.startTime = (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m;
            h = parseInt($("#hh2").val());
            m = parseInt($("#mm2").val());
            pm = $("#ampm2").val() === "PM";
            if (h == 12) h -= 12;
            if (pm) h += 12;
            scheduleEditData.endTime = (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m;
            scheduleEditData.notes = $("#notes").val();
            // check date range
            var days = moment(scheduleEditData.extra.date2).diff(moment(scheduleEditData.extra.date1), "days") + 1;
            var selectedWeekDays = Array.from($("#chkMon:checked,#chkTue:checked,#chkWed:checked,#chkThu:checked,#chkFri:checked,#chkSat:checked,#chkSun:checked")).map(function(el) { return el.id.replace('chk', '') });
            var selectedDates = [];
            if (days > 0) {
                var startDate = moment(scheduleEditData.extra.date1);
                var endDate = moment(scheduleEditData.extra.date2);
                do {
                    if (selectedWeekDays.length > 0 && selectedWeekDays.indexOf(startDate.format('ddd')) !== -1) {
                        selectedDates.push(startDate.format('YYYY-MM-DD'));
                    } else if (selectedWeekDays.length == 0) {
                        selectedDates.push(startDate.format('YYYY-MM-DD'));
                    }
                    startDate = startDate.add("days", 1);
                } while (startDate <= endDate);
            }
            if (selectedDates.length == 0) {
                parent.PopupMessage(null, null, null, "445", true, null, null, false);
                return;
            }
            var r = readers.find(i => i.id === readerId);
            var readerTz = tz.find(i => i.id == r.timeZoneId);

            if (validateNewSchedule(editingTz.text, selectedTimezone, selectedDates, scheduleEditData)) {
                // prepare data
                var dataToSave = {
                    date1: getDate(scheduleEditData.extra.date1, scheduleEditData.startTime, editingTz.text, defaultTz.text),
                    date2: getDate(scheduleEditData.extra.date2, scheduleEditData.startTime, editingTz.text, defaultTz.text),
                    startTime: getTime(scheduleEditData.extra.date1, scheduleEditData.startTime, editingTz.text, defaultTz.text),
                    endTime: getTime(scheduleEditData.extra.date1, scheduleEditData.endTime, editingTz.text, defaultTz.text),
                    days: days,
                    weekDays: [],
                    notes: scheduleEditData.notes,
                    readerId: readerId,
                    userId: parent.objhdnUserID.value,
                    menuId: 0, //parent.objhdnMenuID.value,
                };
                if (days != selectedDates.length && days > 0) {
                    // modify weekdays according to default tz
                    var weekDays = [];
                    selectedDates.forEach(function(d) {
                        var weekDay = moment(`${d} ${scheduleEditData.startTime} ${tzOffsetString(editingTz.offset)} `)
                .utcOffset(defaultTz.offset)
                .day();
                if (weekDays.indexOf(weekDay) == -1)
                    weekDays.push(weekDay);
            });
            dataToSave.weekDays = weekDays;
        }
                createSchedule(dataToSave);
    }
    });
});

});

function makeTimeString(h, m, ampm) {
    var pm = ampm === "PM";
    if (h == 12) h -= 12;
    if (pm) h += 12;
    return (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m;
}

//function setDefaultTimeDisplay(defaultTz, currentTz, date, fromTime, toTime) {
//    var html = "";
//    if (defaultTz.id != currentTz.id) {
//        debugger;
//        const iana_tz = findIana(currentTz.text);
//        debugger;
//        var fromTime = moment(`${date} ${fromTime} ${tzOffsetString(currentTz.offset)}`).utcOffset(defaultTz.offset).format(`${momentDateFormat} LT`);
//        var toTime = moment(`${date} ${toTime} ${tzOffsetString(currentTz.offset)}`).utcOffset(defaultTz.offset).format(`${momentDateFormat} LT`);
//        html = `<span><b>${defaultTz.text}</b></span> <span>From: ${fromTime} to ${toTime}</span>`;
//    }
//    $("#defaultTz").html(html);
//}

function alternateTimeZoneDisplay(zones, currentTz, date, fromTime, toTime) {
    var html = "";
    var defaultTz = zones.find(function(i) { return i.id != currentTz.id; });
    if (!defaultTz) defaultTz = currentTz;
    if (defaultTz.id != currentTz.id) {
        var fromIana = findIana(currentTz.text);
        var targetIana = findIana(defaultTz.text);
        let currentDate = moment.tz(`${date} ${fromTime}`, fromIana[0]);
        const _fromTime = moment.tz(currentDate, targetIana[0]).format(`dddd ${momentDateFormat} LT`);
currentDate = moment.tz(`${date} ${toTime}`, fromIana[0]);
        const _toTime = moment.tz(currentDate, targetIana[0]).format(`dddd ${momentDateFormat} LT`);

html = `<span><b>${defaultTz.text}</b></span> <span>From: ${_fromTime} to ${_toTime}</span>`;
}
$("#defaultTz").html(html);
}

function onChangeReader(scheduleEditData) {
    var defaultTz = tz.find(i => i.isDefault == true);
    var r = readers.find(i => i.id == scheduleEditData.readerId);
    $("#rcolor").html(`<span style="height: 16px; width:16px; background-color:${r.color}; border-radius:50%;display:block;margin-bottom:4px;"></span>`);
    var readerTz = tz.find(i => i.id == r.timeZoneId);
    tzs = [defaultTz];
    if (defaultTz.id != readerTz.id) {
        tzs.push(readerTz);
    }
    $('#currentTz').html('');
    var tzSetting = $("#currentTz").select2({
        data: tzs
    });
    editingTz = readerTz;
    tzSetting.val(editingTz.id).trigger('change');
    
    $("#readertz").html(`<span>${readerTz.text}</span>`);
var currentOffset = tzOffsetString(selectedTz);
var from = moment(range[0]).format("YYYY-MM-DD");
var timestr = Schedule.timeline[0];
var startTime = toTimeString(timestr);
var readerTzDateString = getDate(from, startTime, selectedTimezone, editingTz.text);
//moment(`${from} ${startTime} ${currentOffset}`).utcOffset(editingTz.offset).format("YYYY-MM-DD");
var maxDate = moment(readerTzDateString).add("days", parseInt($("#next").val()) - 1).toDate();
date1 = flatpickr("#date1", {
    defaultDate: moment(readerTzDateString).toDate(),
    dateFormat: ldateFormat,
    minDate: moment(readerTzDateString).toDate(),
    //maxDate: maxDate,
    onClose: function(selectedDates, dateStr, instance) {
        scheduleEditData.date = moment(selectedDates[0]).format("YYYY-MM-DD");
        scheduleEditData.extra.date1 = moment(selectedDates[0]).format("YYYY-MM-DD");
        if (!scheduleEditData.extra.date2) scheduleEditData.extra.date2 = moment(selectedDates[0]).format("YYYY-MM-DD");
        date2.set("minDate", moment(scheduleEditData.extra.date1).toDate());
        if ($("#chkNext").val() == "on") {
            var days = parseInt($("#ndays").val() || "0") || 1;
            date2.setDate(moment(scheduleEditData.extra.date1).add("days", days - 1).toDate());
            scheduleEditData.extra.date2 = moment(scheduleEditData.extra.date1).add("days", days - 1).format('YYYY-MM-DD');
        } else {
            date2.setDate(moment(scheduleEditData.extra.date1).toDate());
            $("#ndays").val("");
        }
        alternateTimeZoneDisplay(tzs, editingTz, scheduleEditData.date, scheduleEditData.startTime, scheduleEditData.endTime);
    }
});
date2 = flatpickr("#date2", {
    defaultDate: moment(readerTzDateString).toDate(),
    dateFormat: ldateFormat,
    minDate: moment(readerTzDateString).toDate(),
    //maxDate: maxDate,
    onClose: function(selectedDates, dateStr, instance) {
        scheduleEditData.extra.date2 = moment(selectedDates[0]).format("YYYY-MM-DD");
        alternateTimeZoneDisplay(tzs, editingTz, scheduleEditData.date, scheduleEditData.startTime, scheduleEditData.endTime);
    }
});
scheduleEditData.date = readerTzDateString;
scheduleEditData.extra.date1 = readerTzDateString;
scheduleEditData.extra.date2 = readerTzDateString;
var readerTime = getTime(from, startTime, selectedTimezone, editingTz.text);
//moment(`${from} ${startTime} ${currentOffset}`).utcOffset(editingTz.offset).format("LT");
scheduleEditData.startTime = readerTime;
scheduleEditData.endTime = readerTime;
alternateTimeZoneDisplay(tzs, editingTz, scheduleEditData.date, scheduleEditData.startTime, scheduleEditData.endTime);
var hasampm = /\d+:\d+\s*(am|pm|AM|PM)/.test(readerTime);
var h, m, ampm;
h = parseInt(readerTime.match(/(\d+):(\d+)/)[1]);
m = parseInt(readerTime.match(/(\d+):(\d+)/)[2]);
if (hasampm) ampm = (readerTime.match(/\d+:\d+\s*(\w+)/)[1]).toUpperCase();

if (!hasampm) {
    if (h > 11) ampm = "PM";
    else ampm = "AM";
    h = h % 12;
    if (h == 0) h += 12;
}
$("#hh1").val(h);
$("#hh2").val(h);
$("#mm1").val(m);
$("#mm2").val(m);
$("#ampm1").val(ampm.toUpperCase());
$("#ampm2").val(ampm.toUpperCase());

$("#chkNext").change(function() {
    date2.set("clickOpens", !this.checked);

    if (this.checked) {
        $("#ndays").removeAttr("readonly");
        var days = 1;
        if (scheduleEditData.extra.date2) {
            days = moment(scheduleEditData.extra.date2).diff(moment(scheduleEditData.extra.date1), "days") + 1;
        }
        $("#ndays").val(days);
        $("#ndays").focus();
    } else {
        $("#ndays").attr("readonly", true);
        var days = parseInt($("#ndays").val() || "0") || 1;

        date2.setDate(moment(scheduleEditData.extra.date1).add("days", days - 1).toDate());
        scheduleEditData.extra.date2 = moment(scheduleEditData.extra.date1).add("days", days - 1).format('YYYY-MM-DD');
        $("#ndays").val("");
    }
});
$("#ndays").change(function(e) {
    if ($("#chkNext").val() == "on") {
        var days = parseInt($("#ndays").val() || "0") || 1;
        date2.setDate(moment(scheduleEditData.extra.date1).add("days", days - 1).toDate());
        scheduleEditData.extra.date2 = moment(scheduleEditData.extra.date1).add("days", days - 1).format('YYYY-MM-DD');
    } else {
        $("#ndays").val("");
    }
});
$("#hh1,#hh2,#mm1,#mm2,#ampm1,#ampm2").on("change", function() {
    if (!scheduleEditData.date) return;
    var readerOffset = tzOffsetString(editingTz.offset);
    var date = scheduleEditData.date;
    var startTime = makeTimeString(parseInt($("#hh1").val()), parseInt($("#mm1").val()), $("#ampm1").val());
    var endTime = makeTimeString(parseInt($("#hh2").val()), parseInt($("#mm2").val()), $("#ampm2").val());
    alternateTimeZoneDisplay(tzs, editingTz, date, startTime, endTime);
});
tzSetting.on('select2:select', function(e) {
    var oldTz = editingTz;
    if (e.params.data.id == "") {
        editingTz = readerTz;
        editingTimezone = editingTz.text;
    }
    else {
        editingTz = tzs.find(function (i) { return i.id == e.params.data.id });
        editingTimezone = editingTz.text;
    }
    var oldTzString = tzOffsetString(oldTz.offset);
    var startTime = makeTimeString(parseInt($("#hh1").val()), parseInt($("#mm1").val()), $("#ampm1").val());
    var endTime = makeTimeString(parseInt($("#hh2").val()), parseInt($("#mm2").val()), $("#ampm2").val());

    var date = getDate(scheduleEditData.extra.date1, startTime, oldTz.text, editingTz.text);
    //moment(`${scheduleEditData.extra.date1} ${startTime} ${oldTzString}`).utcOffset(editingTz.offset).format('YYYY-MM-DD');
    var dt2 = getDate(scheduleEditData.extra.date2, startTime, oldTz.text, editingTz.text);
    //moment(`${scheduleEditData.extra.date2} ${startTime} ${oldTzString}`).utcOffset(editingTz.offset).format('YYYY-MM-DD');
    var from = getTime(scheduleEditData.extra.date1, startTime, oldTz.text, editingTz.text);
    //moment(`${scheduleEditData.extra.date1} ${startTime} ${oldTzString}`).utcOffset(editingTz.offset).format('HH:mm');
    var to = getTime(scheduleEditData.extra.date1, startTime, oldTz.text, editingTz.text);
    //moment(`${scheduleEditData.extra.date1} ${endTime} ${oldTzString}`).utcOffset(editingTz.offset).format('HH:mm');
    if (oldTz.id != editingTz.id) {
        scheduleEditData.date = date;
        scheduleEditData.extra.date1 = date;
        scheduleEditData.extra.date2 = dt2;
        scheduleEditData.startTime = from;
        scheduleEditData.endTime = to;
        var h, m, ampm;
        h = parseInt(from.match(/(\d+):(\d+)/)[1]);
        m = parseInt(from.match(/(\d+):(\d+)/)[2]);

        if (h > 11) ampm = "PM";
        else ampm = "AM";
        h = h % 12;
        if (h == 0) h += 12;

        $("#hh1").val(h);
        $("#mm1").val(m);
        $("#ampm1").val(ampm.toUpperCase());

        h = parseInt(to.match(/(\d+):(\d+)/)[1]);
        m = parseInt(to.match(/(\d+):(\d+)/)[2]);

        if (h > 11) ampm = "PM";
        else ampm = "AM";
        h = h % 12;
        if (h == 0) h += 12;
        $("#hh2").val(h);
        $("#mm2").val(m);

        $("#ampm2").val(ampm.toUpperCase());

    }
    date1 = flatpickr("#date1", {
        defaultDate: moment(date).toDate(),
        dateFormat: ldateFormat,
        minDate: moment(date).toDate(),
        //maxDate: maxDate,
        onClose: function(selectedDates, dateStr, instance) {
            scheduleEditData.date = moment(selectedDates[0]).format("YYYY-MM-DD");
            scheduleEditData.extra.date1 = moment(selectedDates[0]).format("YYYY-MM-DD");
            if (!scheduleEditData.extra.date2) scheduleEditData.extra.date2 = moment(selectedDates[0]).format("YYYY-MM-DD");
            date2.set("minDate", moment(scheduleEditData.extra.date1).toDate());
            if ($("#chkNext").val() == "on") {
                var days = parseInt($("#ndays").val() || "0") || 1;
                date2.setDate(moment(scheduleEditData.extra.date1).add("days", days - 1).toDate());
                scheduleEditData.extra.date2 = moment(scheduleEditData.extra.date1).add("days", days - 1).format('YYYY-MM-DD');
            } else {
                date2.setDate(moment(scheduleEditData.extra.date1).toDate());
                $("#ndays").val("");
            }
            alternateTimeZoneDisplay(tzs, editingTz, scheduleEditData.date, scheduleEditData.startTime, scheduleEditData.endTime);
        }
    });
    date2 = flatpickr("#date2", {
        defaultDate: moment(dt2).toDate(),
        dateFormat: ldateFormat,
        minDate: moment(date1).toDate(),
        //maxDate: maxDate,
        onClose: function(selectedDates, dateStr, instance) {
            scheduleEditData.extra.date2 = moment(selectedDates[0]).format("YYYY-MM-DD");
            alternateTimeZoneDisplay(tzs, editingTz, scheduleEditData.date, scheduleEditData.startTime, scheduleEditData.endTime);
        }
    });
    alternateTimeZoneDisplay(tzs, editingTz, date, from, to);
});
}

$("#suppress").on('change', function(e) {
    if (e.target.value == "all") {
        suppress = false;
    } else {
        suppress = true;
    }
    $(".apply").trigger("click");
});

// initialize schedule component
Schedule.setRange(range[0], range[1]);
Schedule.setSlots(['09am', '09pm']);
Schedule.setTimeLine(['12am', '01am', '02am', '03am', '04am', '05am', '06am', '07am', '08am', '09am', '10am', '11am', '12pm', '01pm', '02pm', '03pm', '04pm', '05pm', '06pm', '07pm', '08pm', '09pm', '10pm', '11pm']);

// Schedule on click event handler
Schedule.onClick(function(e) {
    var key = e.data.key;
    var timelineIndex = e.data.timelineIndex;
    var readerId = e.data.readerId;
    $('.checkbox').prop('checked', false);

    if (e.data.index > -1) {
        var data = e.data.scope.schedule[e.data.key][e.data.index];
        var t = tz.find(i => i.isDefault == true);
        var defaultTz = t;
        var defaultTzName = t.text;
        var targetTz = t.offset;
        $("#defaultTz").html(`<span class="mandatory">* Date and times are in timezone: '${defaultTzName}'</span>`);
        // dialog setup for selected schedule for edit
        setupDialog(function() {
            var r = readers.find(i => i.id == readerId);
            var readerTz = tz.find(i => i.id == r.timeZoneId);

            tzs = [defaultTz];
            if (defaultTz.id != readerTz.id) {
                tzs.push(readerTz);
            }
            $("#currentTz").html("");
            var tzSetting = $("#currentTz").select2({
                data: tzs
            });
            editingTz = readerTz;

            tzSetting.val(editingTz.id).trigger('change');
            $("#readertz").html(`<span>${readerTz.text}</span>`);

        $("#scheduleTitle").text("Modify schedule");
        $("#rcolor").html(`<span style="height: 16px; width:16px; background-color:${r.color}; border-radius:50%;display:block;margin-bottom:4px;"></span>`);
        $('#readerId').html('');
        $("#readerId").select2({
            data: readers.map(function(i) { return { id: i.id, text: i.name, groupId: i.groupId }; })
        });
        $("#readerId").val(e.data.readerId).trigger('change.select2');
        $("#readerId").attr("disabled", "disabled");
        if(userDefaults.roleCode=="RDL"){
            $('#savebtn').hide();
            $('#deletebtn').hide(); 
        }
        else
            $('#deletebtn').show();
        var _cstdate = data.original.date;
        $('.editmode').show();
        $('.newmode').hide();

        var date = getDate(data.original.date, data.original.range[0], defaultTz.text, readerTz.text);
        var from = getTime(data.original.date, data.original.range[0], defaultTz.text, readerTz.text);
        var to = getTime(data.original.date, data.original.range[1], defaultTz.text, readerTz.text);

        // schedule object for holding data for saving
        scheduleEditData = {
            id: data.id,
            readerId: readerId,
            date: date,
            startTime: from,
            endTime: to,
            notes: data.notes,
            userId: parent.objhdnUserID.value,
            menuId: 0 //parent.objhdnMenuID.value
        };
        // date without picker 
        flatpickr("#date", {
            defaultDate: moment(date).toDate(),
            dateFormat: ldateFormat,
            clickOpens: false
        });
        $("#notes").val(data.notes);

        var h, m, ampm;
        h = parseInt(from.match(/(\d+):(\d+)/)[1]);
        m = parseInt(from.match(/(\d+):(\d+)/)[2]);

        if (h > 11) ampm = "PM";
        else ampm = "AM";
        h = h % 12;
        if (h == 0) h += 12;

        $("#hh1").val(h);
        $("#mm1").val(m);
        $("#ampm1").val(ampm.toUpperCase());

        h = parseInt(to.match(/(\d+):(\d+)/)[1]);
        m = parseInt(to.match(/(\d+):(\d+)/)[2]);

        if (h > 11) ampm = "PM";
        else ampm = "AM";
        h = h % 12;
        if (h == 0) h += 12;
        $("#hh2").val(h);
        $("#mm2").val(m);

        $("#ampm2").val(ampm.toUpperCase());
        var readerOffset = tzOffsetString(readerTz.offset);
        var date = scheduleEditData.date;
        var startTime = makeTimeString(parseInt($("#hh1").val()), parseInt($("#mm1").val()), $("#ampm1").val());
        var endTime = makeTimeString(parseInt($("#hh2").val()), parseInt($("#mm2").val()), $("#ampm2").val());
        alternateTimeZoneDisplay(tzs, readerTz, date, startTime, endTime);

        $("#hh1,#hh2,#mm1,#mm2,#ampm1,#ampm2").on("change", function() {
            var readerOffset = tzOffsetString(readerTz.offset);
            var date = scheduleEditData.date;
            var startTime = makeTimeString(parseInt($("#hh1").val()), parseInt($("#mm1").val()), $("#ampm1").val());
            var endTime = makeTimeString(parseInt($("#hh2").val()), parseInt($("#mm2").val()), $("#ampm2").val());
            alternateTimeZoneDisplay(tzs, readerTz, date, startTime, endTime);
        });

        tzSetting.on('select2:select', function(e) {
            var oldTz = editingTz;
            if (e.params.data.id == "")
                editingTz = readerTz;
            else
                editingTz = tzs.find(function(i) { return i.id == e.params.data.id });
            var oldTzString = tzOffsetString(oldTz.offset);
            var startTime = makeTimeString(parseInt($("#hh1").val()), parseInt($("#mm1").val()), $("#ampm1").val());
            var endTime = makeTimeString(parseInt($("#hh2").val()), parseInt($("#mm2").val()), $("#ampm2").val());
            var date = getDate(scheduleEditData.date, startTime, oldTz.text, editingTz.text);
            //moment(`${scheduleEditData.date} ${startTime} ${oldTzString}`).utcOffset(editingTz.offset).format('YYYY-MM-DD');
            var from = getTime(scheduleEditData.date, startTime, oldTz.text, editingTz.text);
            //moment(`${scheduleEditData.date} ${startTime} ${oldTzString}`).utcOffset(editingTz.offset).format('HH:mm');
            var to = getTime(scheduleEditData.date, endTime, oldTz.text, editingTz.text);
            //moment(`${scheduleEditData.date} ${endTime} ${oldTzString}`).utcOffset(editingTz.offset).format('HH:mm');
            if (oldTz.id != editingTz.id) {
                scheduleEditData.date = date;
                scheduleEditData.startTime = from;
                scheduleEditData.endTime = to;
                var h, m, ampm;
                h = parseInt(from.match(/(\d+):(\d+)/)[1]);
                m = parseInt(from.match(/(\d+):(\d+)/)[2]);

                if (h > 11) ampm = "PM";
                else ampm = "AM";
                h = h % 12;
                if (h == 0) h += 12;

                $("#hh1").val(h);
                $("#mm1").val(m);
                $("#ampm1").val(ampm.toUpperCase());

                h = parseInt(to.match(/(\d+):(\d+)/)[1]);
                m = parseInt(to.match(/(\d+):(\d+)/)[2]);

                if (h > 11) ampm = "PM";
                else ampm = "AM";
                h = h % 12;
                if (h == 0) h += 12;
                $("#hh2").val(h);
                $("#mm2").val(m);

                $("#ampm2").val(ampm.toUpperCase());

            }
            flatpickr("#date", {
                defaultDate: moment(date).toDate(),
                dateFormat: ldateFormat,
                clickOpens: false
            });
            alternateTimeZoneDisplay(tzs, editingTz, date, from, to);
        });
        if(userDefaults.roleCode=="RDL"){
            return;
        }
        // save
        $("#savebtn").bind("click", function(ev) {
            if(userDefaults.roleCode=="RDL"){
                return;
            }
            var h = parseInt($("#hh1").val());
            var m = parseInt($("#mm1").val());
            var pm = $("#ampm1").val() === "PM";
            if (h == 12) h -= 12;
            if (pm) h += 12;

            scheduleEditData.startTime = (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m;
            h = parseInt($("#hh2").val());
            m = parseInt($("#mm2").val());
            pm = $("#ampm2").val() === "PM";
            if (h == 12) h -= 12;
            if (pm) h += 12;
            scheduleEditData.endTime = (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m;
            scheduleEditData.notes = $("#notes").val();
            if (validateSchedule(readerTz.text, selectedTimezone, scheduleEditData)) {
                var dataToSave = {
                    id: scheduleEditData.id,
                    readerId: scheduleEditData.readerId,
                    date: getDate(scheduleEditData.date, scheduleEditData.startTime, readerTz.text, defaultTz.text),
                    startTime: getTime(scheduleEditData.date, scheduleEditData.startTime, readerTz.text, defaultTz.text),
                    endTime: getTime(scheduleEditData.date, scheduleEditData.endTime, readerTz.text, defaultTz.text),
                    notes: scheduleEditData.notes,
                    userId: parent.objhdnUserID.value,
                    menuId: 0, //parent.objhdnMenuID.value,
                };
                saveSchedule(dataToSave);
            }
        });
        // delete
        $('#deletebtn').bind("click", function(ev) {
            if(userDefaults.roleCode=="RDL"){
                return;
            }
            deleteSchedule(scheduleEditData);
        });
    });
}

});

$("#editDialog").on('hide.bs.modal', function() {

    $("#savebtn").unbind("click");
    $("#deletebtn").unbind("click");
    $('.checkbox').prop('checked', false);
    $("#hh1,#hh2,#mm1,#mm2,#ampm1,#ampm2").unbind("change");
    $("#scheduletable").focus();
});

$(".apply").on("click", function() {
    PopupProcess();
    $.ajax({
        url: baseUrl + "Radiologist/ScheduleWebServices.asmx/GetSchedules?" +
            `tz=${selectedTimezone}&from=${moment(range[0]).format("YYYY-MM-DD")}&to=${moment(range[1]).format("YYYY-MM-DD")}&readerId=${selectedReader || ''}&readerGroupId=${selectedGroup || 0}`,
dataType: 'json',
    success: function(result) {
        Schedule.setRange(range[0], range[1]);
        Schedule.setData(result.schedules || {});
        Schedule.setStats(result.stats||{});
        Schedule.showScheuled(suppress);

        Schedule.setShowByGroupColor((mode == "group" && selectedGroup == null));
        Schedule.setPerformance(result.performance || []);
        Schedule.paint();
        $('[data-tooltip="true"]').tooltip({
            container: 'body',
            html: true
        });
        HideProcess();
    },
failure: function() {
    HideProcess();
}
});
});
$.ajax({
    url: baseUrl + "Radiologist/ScheduleWebServices.asmx/GetParameters?userId=" + parent.objhdnUserID.value,
    dataType: 'json',
    success: function(result) {
        userDefaults=result.roleInfo;
        if(userDefaults.defaultReaderId=='00000000-0000-0000-0000-000000000000') userDefaults.defaultReaderId=null;
        tz = result.timezones.map(function(i) { return { id: i.id, offset: i.offset, text: i.name, isDefault: i.isDefault == 'Y' }; });
        $("#tz").select2({
            data: [...tz]
        });
        var t = tz.find(i => i.isDefault == true);
        if (t) {
            selectedTz = t.offset;
            selectedTimezone = t.text;
        }
       
        if(userDefaults.defaultReaderId && userDefaults.timeZoneId != t.id){
            t = tz.find(i => i.id == userDefaults.timeZoneId);
            if (t) {
                selectedTz = t.offset;
                selectedTimezone = t.text;
            }
        }

        $("#tz").val(t.id).trigger('change');
        readers = result.readers;
        readerGroups = result.groups;

        $("#group").select2({
            data: readerGroups.map(function(i) { return { id: i.id, text: i.name, color: i.color }; })
        });
        selectedGroup = null;

        if(userDefaults.defaultReaderId){
            $("#option").css("display", "none");
            $("#option").val("reader").trigger("change");
            $("#readers").css("display", "");
            $("#groups").css("display", "none");

        } else {
            $("#group").val(selectedGroup).trigger('change');
            $("#groups").css("display", "");
            $("#readers").css("display", "none");
        }
        
        $("#reader").select2({
            data: readers.map(function(i) { return { id: i.id, text: i.name, groupId: i.groupId }; })
        });

        if(userDefaults.defaultReaderId){
            selectedReader = userDefaults.defaultReaderId;
            $("#reader").attr("disabled", "disabled");
            $("#reader").val(selectedReader).trigger('change');
            Schedule.setShowByGroupColor(false);
        }
        else{
            if (!selectedGroup) {
                Schedule.setReaders(readers);
                Schedule.setShowByGroupColor(true);
            } else {
                Schedule.setReaders(readers.filter(i => i.groupId == selectedGroup));
                Schedule.setShowByGroupColor(false);
            }
        }
        if(userDefaults.roleCode=="RDL"){
            $("#create").hide();
        }
        $(".apply").trigger("click");
    }
});

function createSchedule(data) {
    $.ajax({
        url: baseUrl + "Radiologist/ScheduleWebServices.asmx/CreateSchedule",
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
    }).done(function(result) {
        // result.d contains the response
        $('#editDialog').modal('hide');
        $('#readerId').off('select2:select');
        $(".apply").trigger("click");
    }).error(function(response) {
        var result = response.responseJSON.d;
        if (result.status == "error") {
            parent.PopupMessage(null, null, null, result.returnMessage || result.catchMessage, true, null, null, false);
        }
    });
}

function saveSchedule(data) {
    $.ajax({
        url: baseUrl + "Radiologist/ScheduleWebServices.asmx/SaveSchedule",
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
    }).done(function(result) {
        // result.d contains the response
        $('#editDialog').modal('hide');
        $('#readerId').off('select2:select');
        $(".apply").trigger("click");
    }).error(function(response) {
        var result = response.responseJSON.d;
        if (result.status == "error") {
            parent.PopupMessage(null, null, null, result.returnMessage || result.catchMessage, true, null, null, false);
        }
    });
}

function deleteSchedule(data) {
    $.ajax({
        url: baseUrl + "Radiologist/ScheduleWebServices.asmx/DeleteSchedule",
        type: "GET",
        data: { id: data.id, userId: data.userId, menuId: data.menuId },
        dataType: 'json'
    }).done(function(result) {
        $('#editDialog').modal('hide');
        $('#readerId').off('select2:select');
        $(".apply").trigger("click");
    });
}

function validateSchedule(readerTzname, currentTzname, editdata) {
    var key = getDate(editdata.date, editdata.startTime, readerTzname, currentTzname);
    var from = getTime(editdata.date, editdata.startTime, readerTzname, currentTzname);
    var to = getTime(editdata.date, editdata.endTime, readerTzname, currentTzname);
    var dataobj = Schedule.schedule[key];
    if (dataobj) {
        var timeSegments = dataobj.filter(i => i.readerId == scheduleEditData.readerId && i.id != scheduleEditData.id).map(function(i) { return i.range });
        timeSegments.push([from, to]);
        var overlapping = checkTimeRangeOverlapping(timeSegments);
        if (overlapping) {
            parent.PopupMessage(null, null, null, "441", true, null, null, false);
            return false;
        }
    }
    return true;
}

function validateNewSchedule(readerTzName, currentTzName, dates, editdata) {
    if (dates.length > 0) {
        for (var d = 0; d < dates.length; d++) {
            var key = getDate(dates[d], editdata.startTime, readerTzName, currentTzName);
            var from = getTime(dates[d], editdata.startTime, readerTzName, currentTzName);
            var to = getTime(dates[d], editdata.endTime, readerTzName, currentTzName);
            var dataobj = Schedule.schedule[key];
            if (dataobj) {
                var timeSegments = dataobj.filter(i => i.readerId == scheduleEditData.readerId && i.id != scheduleEditData.id).map(function(i) { return i.range });
                timeSegments.push([from, to]);
                var overlapping = checkTimeRangeOverlapping(timeSegments);
                if (overlapping) {
                    parent.PopupMessage(null, null, null, "441", true, null, null, false);
                    return false;
                }
            }
        }
    }

    return true;
}

function tzOffsetString(offset) {
    const s = parseFloat(offset).toFixed(2).toString();
    const h = parseFloat(s.match(/(-?\d+)(\.\d+)/)[1]);
    const m = parseFloat(s.match(/(-?\d+)(\.\d+)/)[2]) * 60;
    return "UTC" + ((h < 0) ? "-" : "+") + (Math.abs(h) < 10 ? "0" : "") + Math.abs(h) + ":" + (m < 10 ? "0" : "") + m;
}

function toTimeString(timeline) {
    var h = parseInt(timeline.match(/(\d+)(am|pm)/)[1]);
    var ampm = timeline.match(/(\d+)(am|pm)/)[2];
    if (h == 12) h = 0;
    if (ampm == "pm") h += 12;
    return (h < 10 ? "0" : "") + h + ":00";
}

function getHours(time) {
    var h = parseInt(time.match(/(\d+):(\d+)/)[1]);
    var m = parseInt(time.match(/(\d+):(\d+)/)[2]);
    if (m >= 30) h += 1;
    if (h > 24) h = 24;
    m = 0;
    var hh = h % 12;
    if (hh == 0) hh = 12;
    var ampm = "am";
    if (h > 11 && h < 24) ampm = "pm";
    return (hh > 9 ? hh.toString() : "0" + hh) + ampm;
}

function setupDialog(action) {
    action();
    $('#editDialog').modal('show');

}
/*
    compare if there are any overlapping time range
    var timeSegments = [
      ["03:00", "04:00"],
      ["02:00", "07:00"],
      ["12:00", "15:00"]
    ];
*/
function checkTimeRangeOverlapping(timeSegments) {
    if (timeSegments.length === 1) return false;

    timeSegments.sort((timeSegment1, timeSegment2) =>
        timeSegment1[0].localeCompare(timeSegment2[0])
    );

    for (let i = 0; i < timeSegments.length - 1; i++) {
        const currentEndTime = timeSegments[i][1];
        const nextStartTime = timeSegments[i + 1][0];

        if (currentEndTime > nextStartTime) {
            return true;
    }
}

return false;
}

// timezone converted date
function getDate(date, time, defaultTzName, targetTzName) {
    var defaultIana = findIana(defaultTzName);
    var targetIana = findIana(targetTzName);
    var defaultDate = moment.tz(`${date} ${time}`, defaultIana[0]);
var targetFormattedDate = moment.tz(defaultDate, targetIana[0]).format('YYYY-MM-DD');
return targetFormattedDate;
}
// timezone converted time
function getTime(date, time, defaultTzName, targetTzName) {
    var defaultIana = findIana(defaultTzName);
    var targetIana = findIana(targetTzName);
    var defaultDate = moment.tz(`${date} ${time}`, defaultIana[0]);
var targetFormattedTime = moment.tz(defaultDate, targetIana[0]).format('HH:mm');
return targetFormattedTime;
}