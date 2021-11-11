(function() {
    function Schedule() {
        this.timeline = [];
        this.toDate = new Date(new Date() + 7 * 24 * 3600 * 1000);
        this.fromDate = new Date();
        this.slots = [];
        this.stats = {};
        this.weekdays = [];
        this.radioligists = [];
        this.schedule = {};
        this.performance = [];
        this.onClickEvent = null;
        this.showByGroupColor = false;
        this.showScheuledOnly = false;
    }
    Schedule.prototype.setShowByGroupColor = function(value) {
        this.showByGroupColor = value;
    };
    Schedule.prototype.onClick = function(func) {
        this.onClickEvent = func;
    };
    Schedule.prototype.setRange = function(fromDate, toDate) {
        this.fromDate = fromDate;
        this.toDate = toDate;
        this.weekdays = this.getDays();
    };
    Schedule.prototype.setSlots = function(slots) {
        this.slots = slots || [];
    };
    Schedule.prototype.setStats = function(stats) {
        this.stats = stats || {};
    };
    Schedule.prototype.setPerformance = function(performance) {
        this.performance = performance || [];
    };
    Schedule.prototype.setTimeLine = function(timeline) {
        this.timeline = timeline || [];
    };
    Schedule.prototype.setReaders = function(readers) {
        this.radioligists = readers || [];
    };
    Schedule.prototype.setData = function(data) {
        this.schedule = data || [];
    };
    Schedule.prototype.showScheuled = function(data) {
        this.showScheuledOnly = data;
    };
    /*
	   expected:
	   {
			timeline:['12am', '01am', '02am',...,'11pm'],
			slots:['09am','09pm'],
			range:['2020-11-30', '2020-12-06'],
			
			
			schedule:{
				'2020-11-30':[{
					"id": "B1FE99F1-54F6-4217-B91B-0111CC34ACB8",
        			"readerId": "9D464E19-E928-46DB-BE7A-775BDDF7F858",
					range:['02:00', '03:00'],
					original:{
						date:'2020-11-30',
						offset: -6,
						range:['02:00:00', '03:00:00']
					}
				}, {
					"id": "41C58DB8-19AF-4A12-9A03-0113AB89B86C",
        			"readerId": "E53BA58E-5263-4A51-B050-CC3AA821FC45",
					range:['00:00', '03:00'],
					original:{
						date:'2020-11-30',
						offset: -6,
						range:['00:00', '03:00']
					}
				}],
				
				...
			}
	   }

       stats: {
            '2020-11-30':[{
                    timelineIndex: 0, count: 5
                }, {
                    timelineIndex: 5, count: 15
                }],
            '2020-12-01':[{
                    timelineIndex: 10, count: 25
                }, {
                    timelineIndex: 15, count: 5
                }],
       }
	*/
    Schedule.prototype.showLegends = function() {
        return this.radioligists.length > 1;
    };
    Schedule.prototype.showTotalColumn = function() {
        return this.radioligists.length > 1;
    };
    Schedule.prototype.showReader = function() {
        return this.radioligists.length > 1;
    };
    Schedule.prototype.showStat = function(day) {
        if (this.radioligists.length == 1) {
            return this.weekdays.indexOf(day) === 0;
        }
        if (this.radioligists.length === 0) return false;
        return true;
    };

    /*
    *  when multiple readers, 
    *       header row:  
    			Tuesday, December 1, 2020 (reader|group>6)
    			Tue, Dec 1, 20 when readers|group 1 - 6
    *
    */
    Schedule.prototype.createHeader = function(table) {
        var o = this;
        // create header
        var header = table.createTHead();
        var samerowstat = o.radioligists.length == 1;
        var statDrawn = false;
        var dailyRadiologists = this.dateWiseReader();
        // first header row
        var row = header.insertRow(0);
        var cell = document.createElement("th");
        if (samerowstat) {

            var firstday = moment(o.weekdays[0].calendarDate).format("Do MMM");
            var lastday = moment(o.weekdays[o.weekdays.length - 1].calendarDate).format("Do MMM");
            if (firstday.match(/(\d+(st|nd|rd|th)) (\w{3})/)[3] == lastday.match(/(\d+(st|nd|rd|th)) (\w{3})/)[3]) {
                cell.innerText = firstday.match(/(\d+(st|nd|rd|th)) (\w{3})/)[1] + "-" + lastday;
            } else
                cell.innerText = firstday + "-" + lastday;

        }
        row.appendChild(cell);
        

        o.weekdays.forEach(function(w, windex) {
            var radioligists = [];
            if (o.showScheuledOnly)
                radioligists = o.radioligists.filter(function(i) { return (dailyRadiologists[w.calendarDate] || []).indexOf(i.id) >= 0; });
            // stat
            let span = 0;
            if (o.showStat(w) && !samerowstat) {
                span += (3+1);
            }
            if (samerowstat && !statDrawn) {
                cell = document.createElement("th");
                cell.className="stat-h";
                cell.setAttribute("id", `h0_d${windex}_stat1`);
                cell.innerText = "Stat 1hr";
                row.appendChild(cell);

                cell = document.createElement("th");
                cell.className = "stat-h";
                cell.setAttribute("id", `h0_d${windex}_stat2`);
                cell.innerText = "Stat 2hrs";
                row.appendChild(cell);

                cell = document.createElement("th");
                cell.className="prelim";
                cell.setAttribute("id", `h0_d${windex}_prelim`);
                cell.innerText = "Prel";
                cell.setAttribute("title", "Preliminary");
                cell.setAttribute("data-tooltip", true);
                row.appendChild(cell);

                cell = document.createElement("th");
                cell.className="coverage";
                cell.setAttribute("id", `h0_d${windex}_coverage`);
                cell.innerText = "Covg";
                cell.setAttribute("title", "Coverage");
                cell.setAttribute("data-tooltip", true);
                row.appendChild(cell);
                statDrawn = true;
            }
            if (o.showScheuledOnly) {
                span += radioligists.length;
            } else {
                span += o.radioligists.length;
            }
            // total
            if (o.showTotalColumn()) {
                span += (1+1);
            }

            cell = document.createElement("th");
            cell.setAttribute("id", `h0_d${windex}`);
            if (span > 6) {
                cell.innerText = moment(w.calendarDate).format("dddd, Do MMMM, YYYY");
            } else if (span > 3) {
                cell.innerText = moment(w.calendarDate).format("ddd DD MMM");
            } else {
                if (samerowstat) {
                    cell.innerHTML = `<span>${moment(w.calendarDate).format("dd")}</span><br/><span style="font-size:8px;">${moment(w.calendarDate).format("Do")}</span>`;
                } else {
                    cell.innerText = moment(w.calendarDate).format("dd");
                }
            }
            cell.colSpan = span == 0 ? 1 : span;
            cell.className = "r-divider";
            row.appendChild(cell);
        });

        // second header row
        if (o.showReader()) {
            row = header.insertRow();
            cell = document.createElement("th");
            cell.innerText = ('Reader');
            row.appendChild(cell);
            o.weekdays.forEach(function(w, windex) {
                var radioligists = [];
                if (o.showScheuledOnly)
                    radioligists = o.radioligists.filter(function(i) { return (dailyRadiologists[w.calendarDate] || []).indexOf(i.id) >= 0; });
                // stat
                if (o.showStat(w)) {
                    cell = document.createElement("th");
                    cell.className="stat-h";
                    cell.setAttribute("id", `h1_d${windex}_stat1`);
                    cell.innerText = "Stat 1hr";
                    row.appendChild(cell);

                    cell = document.createElement("th");
                    cell.className = "stat-h";
                    cell.setAttribute("id", `h1_d${windex}_stat2`);
                    cell.innerText = "Stat 2hrs";
                    row.appendChild(cell);

                    cell = document.createElement("th");
                    cell.className="prelim";
                    cell.setAttribute("id", `h1_d${windex}_prelim`);
                    cell.innerText = "Prel";
                    row.appendChild(cell);
                    cell = document.createElement("th");
                    cell.className="coverage";
                    cell.setAttribute("id", `h1_d${windex}_coverage`);
                    cell.innerText = "Covg";
                    row.appendChild(cell);
                }
                if (o.showScheuledOnly) {
                    radioligists.forEach(function(r) {
                        cell = document.createElement("th");
                        cell.setAttribute("id", `h1_d${windex}_${r.id}`);
                        cell.innerText = r.name.substring(0, 1);
                        cell.setAttribute("title", r.name);
                        cell.setAttribute("data-tooltip", true);
                        row.appendChild(cell);
                    });
                } else {
                    o.radioligists.forEach(function(r) {
                        cell = document.createElement("th");
                        cell.setAttribute("id", `h1_d${windex}_${r.id}`);
                        cell.innerText = r.name.substring(0, 1);
                        cell.setAttribute("title", r.name);
                        cell.setAttribute("data-tooltip", true);
                        row.appendChild(cell);
                    });
                }

                // total
                if (o.showTotalColumn()) {
                    cell = document.createElement("th");
                    cell.setAttribute("id", `h1_d${windex}_cases`);
                    cell.innerText = "Cases";
                    cell.className = "total-h";
                    row.appendChild(cell);

                    cell = document.createElement("th");
                    cell.setAttribute("id", `h1_d${windex}_total`);
                    cell.innerText = "Total";
                    cell.className = "total-h r-divider";
                    row.appendChild(cell);
                }
            });
        }
    };
    Schedule.prototype.createBody = function(table) {
        var o = this;
        var body = document.createElement("tbody");
        table.appendChild(body);
        var dailyRadiologists = this.dateWiseReader();
        o.timeline.forEach(function(t, tindex, tarray) {
            var row = body.insertRow();
            if (o.slots.indexOf(t) != -1)
                row.className = "b-divider";
            var cell = document.createElement("td");
            if (tindex < tarray.length - 1) {
                cell.innerText = `${t} to ${tarray[tindex+1]}`;
            } else {
                cell.innerText = `${t} to ${tarray[0]}`;
            }

            row.appendChild(cell);

            o.weekdays.forEach(function(w, windex) {
                var radioligists = [];
                if (o.showScheuledOnly)
                    radioligists = o.radioligists.filter(function(i) { return (dailyRadiologists[w.calendarDate] || []).indexOf(i.id) >= 0; });
                // stat
                if (o.showStat(w)) {
                    var cell1 = document.createElement("td");
                    cell1.setAttribute("id", `b_d${windex}_stat1_${tindex}`);
                    cell1.className = "stat";
                    var cell2 = document.createElement("td");
                    cell2.setAttribute("id", `b_d${windex}_stat2_${tindex}`);
                    cell2.className = "stat";
                    let stats = o.stats[w.calendarDate] || [];
                    var statFound = stats.find(function(i) { return i.timelineIndex == tindex });
                    if (statFound) {
                        cell1.innerText = statFound.count1;
                        cell2.innerText = statFound.count2;
                    }
                    else{
                        cell1.innerText = 0;
                        cell2.innerText = 0;
                    }
                    cell1.style.textAlign = "center";
                    cell2.style.textAlign = "center";
                    row.appendChild(cell1);
                    row.appendChild(cell2);

                    cell = document.createElement("td");
                    cell.setAttribute("id", `b_d${windex}_prelim_${tindex}`);
                    cell.className="prelim";
                    row.appendChild(cell);

                    cell = document.createElement("td");
                    cell.setAttribute("id", `b_d${windex}_coverage_${tindex}`);
                    cell.className="coverage";
                    cell.style.textAlign = "center";
                    row.appendChild(cell);
                }
                if (o.showScheuledOnly) {
                    if (radioligists.length == 0 && o.radioligists.length==1) {
                        cell = document.createElement("td");
                        cell.setAttribute("id", `b_d${windex}_${o.radioligists[0].id}_${tindex}`);
                        cell.setAttribute("data-key", w.calendarDate);
                        row.appendChild(cell);
                    } else {
                        radioligists.forEach(function (r) {
                            cell = document.createElement("td");
                            cell.setAttribute("id", `b_d${windex}_${r.id}_${tindex}`);
                            cell.setAttribute("data-key", w.calendarDate);
                            row.appendChild(cell);
                        });
                    }
                } else {
                    o.radioligists.forEach(function(r) {
                        cell = document.createElement("td");
                        cell.setAttribute("id", `b_d${windex}_${r.id}_${tindex}`);
                        cell.setAttribute("data-key", w.calendarDate);
                        row.appendChild(cell);
                    });
                }

                // total
                if (o.showTotalColumn()) {
                    cell = document.createElement("td");
                    cell.setAttribute("id", `b_d${windex}_cases_${tindex}`);
                    row.appendChild(cell);
                    cell = document.createElement("td");
                    cell.setAttribute("id", `b_d${windex}_total_${tindex}`);
                    cell.className = "r-divider";
                    row.appendChild(cell);
                }
            });
        });
    };
    Schedule.prototype.createFooter = function(table) {
        var o = this;
        var dailyRadiologists = this.dateWiseReader();
        var footer = document.createElement("tfoot");
        table.appendChild(footer);
        var row = footer.insertRow();
        var cell = document.createElement("td");
        cell.innerHTML = `<span><b>Total</b></span>`;
        row.appendChild(cell);

        o.weekdays.forEach(function(w, windex) {
            var radioligists = [];
            if (o.showScheuledOnly)
                radioligists = o.radioligists.filter(function(i) { return (dailyRadiologists[w.calendarDate] || []).indexOf(i.id) >= 0; });
            // stat
            if (o.showStat(w)) {
                var cell1 = document.createElement("td");
                cell1.className="stat";
                cell1.setAttribute("id", `f0_d${windex}_stat1`);
                row.appendChild(cell1);
                var cell2 = document.createElement("td");
                cell2.className = "stat";
                cell2.setAttribute("id", `f0_d${windex}_stat2`);
                row.appendChild(cell2);
                let stats = o.stats[w.calendarDate] || [];
                var count1 = stats.map(function(i) { return i.count1; }).reduce((s, c) => {
                    return s + parseInt(c); // return the sum of the accumulator and the current time, as the the new accumulator
                }, 0);
                var count2 = stats.map(function (i) { return i.count2; }).reduce((s, c) => {
                    return s + parseInt(c); // return the sum of the accumulator and the current time, as the the new accumulator
                }, 0);
                cell1.innerHTML = `<b>${count1||0}</b>`;
                cell1.style.textAlign = "center";
                cell2.innerHTML = `<b>${count2 || 0}</b>`;
                cell2.style.textAlign = "center";

                cell = document.createElement("td");
                cell.className="prelim";
                cell.setAttribute("id", `f0_d${windex}_prelim`);
                row.appendChild(cell);
                cell.style.textAlign = "center";

                cell = document.createElement("td");
                cell.className="coverage";
                cell.setAttribute("id", `f0_d${windex}_coverage`);
                row.appendChild(cell);
                cell.style.textAlign = "center";
            }
            if (o.showScheuledOnly) {
                if (radioligists.length == 0 && o.radioligists.length == 1) {
                    cell = document.createElement("td");
                    cell.setAttribute("id", `f0_d${windex}_${o.radioligists[0].id}`);
                    row.appendChild(cell);
                } else {
                    radioligists.forEach(function (r) {
                        cell = document.createElement("td");
                        cell.setAttribute("id", `f0_d${windex}_${r.id}`);
                        row.appendChild(cell);
                    });
                }
            } else {
                o.radioligists.forEach(function(r) {
                    cell = document.createElement("td");
                    cell.setAttribute("id", `f0_d${windex}_${r.id}`);
                    row.appendChild(cell);
                });
            }

            // total
            if (o.showTotalColumn()) {
                cell = document.createElement("td");
                cell.setAttribute("id", `f0_d${windex}_cases`);
                row.appendChild(cell);
                cell = document.createElement("td");
                cell.setAttribute("id", `f0_d${windex}_total`);
                cell.className = "r-divider";
                row.appendChild(cell);
            }
        });

        //row = footer.insertRow();
        //cell = document.createElement("td");
        //cell.innerText = (`Left over previous day`);
        //row.appendChild(cell);

        //o.weekdays.forEach(function(w, windex) {

        //    var radioligists = [];
        //    if (o.showScheuledOnly)
        //        radioligists = o.radioligists.filter(function(i) { return (dailyRadiologists[w.calendarDate] || []).indexOf(i.id) >= 0; });

        //    // stat
        //    if (o.showStat(w)) {
        //        cell = document.createElement("td");
        //        cell.setAttribute("id", `f1_d${windex}_stat`);
        //        row.appendChild(cell);

        //        cell = document.createElement("td");
        //        cell.className="prelim";
        //        cell.setAttribute("id", `f1_d${windex}_prelim`);
        //        row.appendChild(cell);
        //        cell.style.textAlign = "center";

        //        cell = document.createElement("td");
        //        cell.className="coverage";
        //        cell.setAttribute("id", `f1_d${windex}_coverage`);
        //        row.appendChild(cell);
        //        cell.style.textAlign = "center";
        //    }
        //    if (o.showScheuledOnly) {
        //        radioligists.forEach(function(r) {
        //            cell = document.createElement("td");
        //            cell.setAttribute("id", `f1_d${windex}_${r.id}`);
        //            row.appendChild(cell);
        //        });
        //    } else {
        //        o.radioligists.forEach(function(r) {
        //            cell = document.createElement("td");
        //            cell.setAttribute("id", `f1_d${windex}_${r.id}`);
        //            row.appendChild(cell);
        //        });
        //    }
        //    // total
        //    if (o.showTotalColumn()) {
        //        cell = document.createElement("td");
        //        cell.setAttribute("id", `f1_d${windex}_total`);
        //        cell.className = "r-divider";
        //        row.appendChild(cell);
        //    }
        //});



        // radioligist legend
        if (o.showLegends()) {
            row = footer.insertRow();
            cell = document.createElement("td");
            cell.innerHTML = '';
            row.appendChild(cell);

            o.weekdays.forEach(function(w, windex) {
                var radioligists = [];
                if (o.showScheuledOnly)
                    radioligists = o.radioligists.filter(function(i) { return (dailyRadiologists[w.calendarDate] || []).indexOf(i.id) >= 0; });

                // stat
                if (o.showStat(w)) {
                    cell = document.createElement("td");
                    cell.setAttribute("id", `f2_d${windex}_stat1`);
                    cell.className="stat";
                    row.appendChild(cell);

                    cell = document.createElement("td");
                    cell.setAttribute("id", `f2_d${windex}_stat2`);
                    cell.className = "stat";
                    row.appendChild(cell);

                    cell = document.createElement("td");
                    cell.className="prelim";
                    cell.setAttribute("id", `f2_d${windex}_prelim`);
                    row.appendChild(cell);
                    cell.style.textAlign = "center";

                    cell = document.createElement("td");
                    cell.className="coverage";
                    cell.setAttribute("id", `f2_d${windex}_coverage`);
                    row.appendChild(cell);
                    cell.style.textAlign = "center";
                }
                
                if (o.showScheuledOnly) {
                    radioligists.forEach(function(r) {
                        //var r = o.radioligists.find(function(i) { return i.id == x; });
                        cell = document.createElement("td");
                        cell.style.verticalAlign = "top";
                        cell.setAttribute("id", `f2_d${windex}_${r.id}`);
                        cell.innerHTML = `<span style="height: 16px; width:16px; background-color:${o.showByGroupColor? r.groupColor:r.color}; border-radius:50%;display:block;margin-bottom:4px;"></span><span class="rotate">${r.name}</span> `;
                        row.appendChild(cell);
                    });
                } else {
                    o.radioligists.forEach(function(r) {
                        cell = document.createElement("td");
                        cell.style.verticalAlign = "top";
                        cell.setAttribute("id", `f2_d${windex}_${r.id}`);
                        cell.innerHTML = `<span style="height: 16px; width:16px; background-color:${o.showByGroupColor? r.groupColor:r.color}; border-radius:50%;display:block;margin-bottom:4px;"></span><span class="rotate">${r.name}</span> `;
                        row.appendChild(cell);
                    });
                }
                // total
                cell = document.createElement("td");
                cell.setAttribute("id", `f2_d${windex}_cases`);
                cell.setAttribute("data-day", w.calendarDate);
                row.appendChild(cell);

                cell = document.createElement("td");
                cell.setAttribute("id", `f2_d${windex}_total`);
                cell.setAttribute("data-day", w.calendarDate);
                cell.className = "r-divider";
                row.appendChild(cell);
            });
        }
    };
    Schedule.prototype.clearMark = function(hash) {
        var o = this;
        var cells = document.querySelectorAll(`td[data-hash="${hash}"]`);
        var dayIndexes = [];
        var readerId = cells[0].getAttribute("id").match(/b_d(\d+)_(\w+-\w+-\w+-\w+-\w+)_(\d+)/)[2];
        cells.forEach(function(cell) {
            var dayIndex = parseInt(cell.getAttribute("id").match(/b_d(\d+)_(\w+-\w+-\w+-\w+-\w+)_(\d+)/)[1]);
            if (dayIndexes.indexOf(dayIndex) == -1) dayIndexes.push(dayIndex);
            cell.removeAttribute("data-index");
            cell.removeAttribute("data-hash");
            cell.removeAttribute("style");
            cell.innerText = "";
        });
        dayIndexes.forEach(function(i) {
            o.calculatePartialTotal(i, readerId);
        });
    }
    Schedule.prototype.mark = function(date, data, index) {

        var o = this;
        var readerId = data.readerId;

        var d = o.weekdays.findIndex(function(i) { return i.calendarDate == date; });
        if (d == -1) return;
        var indexes = getTimeIndexes(o.timeline, data.range[0], data.range[1]);
        var startRowIndex = indexes[0];
        var endRowIndex = indexes[1];

        var nextday = false;
        var nextStart = -1,
            nextEnd = -1;
        if (endRowIndex < startRowIndex) {
            if (endRowIndex >= 0) {
                nextday = true;
                nextStart = 0;
                nextEnd = endRowIndex; //??
            }
            endRowIndex = o.timeline.length - 1;
        }
        var rad = o.radioligists.find(function(i) { return i.id == readerId });
        if (!rad) return;
        var perf = o.performance.find(function(i) { return i.id == readerId });
        if (perf) rad.thresholdPerHr = perf.studyCountPerHr || 0;
        o.paintMark(rad, d, startRowIndex, endRowIndex, date, index, data);
        if (nextday) {
            var nextdate = moment(date).add(1, "days").format("YYYY-MM-DD");
            d = o.weekdays.findIndex(function(i) { return i.calendarDate == nextdate; });
            if (d == -1) return;
            endRowIndex = nextEnd;
            startRowIndex = nextStart;
            o.paintMark(rad, d, startRowIndex, endRowIndex, date, index, data);
        }
    };

    Schedule.prototype.paintMark = function(rad, d, startRowIndex, endRowIndex, date, dataIndex, data) {
        var o = this;
        var count = Math.round(parseFloat(endRowIndex - startRowIndex + 1.0) * parseFloat(rad.thresholdPerHr || '0'), 0);
        var color = o.showByGroupColor ? rad.groupColor : rad.color;
        for (var r = startRowIndex; r <= endRowIndex; r++) {
            var cell = document.getElementById(`b_d${d}_${rad.id}_${r}`);
            if (!cell) continue;
            cell.setAttribute("data-index", dataIndex);
            cell.setAttribute("data-hash", `${+new Date(date)}.${dataIndex}`);
            var shade = shadeColor(color,15);
            
            if(rad.rights==0){
                cell.style.backgroundColor = color;
            }
            else if(rad.rights==2){
                cell.style.background = `linear-gradient(90deg, ${color} 2px, transparent 5%) center,
		                                linear-gradient(${color} 2px, transparent 5%) center,
		                                ${shade}`;
                cell.style.backgroundSize = "9px 9px";
            }else if(rad.rights==1){
                cell.style.background = `repeating-linear-gradient( -45deg, ${color}, ${color} 10px, ${shade} 10px, ${shade} 20px )`;
            }
            
            cell.style.color = getContrast(color);
            if (startRowIndex == r) {
                cell.innerText = count;
                cell.style.textAlign = "center";
                cell.setAttribute("data-tooltip", true);

                if (data.notes) {
                    cell.setAttribute("title", `<span style="height: 12px; width:12px; background-color:${color}; border:1px solid white; border-radius:50%;display:inline-block;margin-right:2px;"></span><span style="display:inline-block;">${rad.name}</span><br/><span style="font-size: 10px; font-style: normal;">Notes: ${data.notes}</span>`);
                } else
                    cell.setAttribute("title", `<span style="height: 12px; width:12px; background-color:${color}; border:1px solid white; border-radius:50%;display:inline-block;margin-right:2px;"></span><span style="display:inline-block;">${rad.name}</span>`);
            }
            if (endRowIndex - startRowIndex > 0 && r < endRowIndex) {
                cell.style.borderBottomColor = color;
                if (r > startRowIndex)
                    cell.style.borderTopColor = color;
            }
            if (endRowIndex - startRowIndex > 0 && r > startRowIndex && r <= endRowIndex) {
                cell.style.borderTopColor = color;
            }

        }
    }

    Schedule.prototype.markAll = function() {
        var o = this;
        var schedules = o.schedule;
        for (var date in schedules) {
            var daydata = schedules[date];
            daydata.forEach(function(data, index) {
                o.mark(date, data, index);
            });
        }
    };
    Schedule.prototype.dateWiseReader = function() {
        var o = this;
        var schedules = o.schedule;
        var dateReader = {};
        for (var date in schedules) {
            var readers = [..._.uniq(schedules[date].map(function (r) { return r.readerId }))];
            debugger;
            if (readers.length > 0) {
                if (!dateReader[date]) dateReader[date] = [];
                dateReader[date] = [..._.uniq([...readers, ...dateReader[date]])];
                // find boundary times

                schedules[date].forEach(function (data, index) {
                    var time1 = moment(`${date} ${data.range[0]}`).toDate();
                    var time2 = moment(`${date} ${data.range[1]}`).toDate();
                    if (time1 > time2) {
                        var nextDate = moment(`${date}`).add("days", 1).format('YYYY-MM-DD');
                        if (dateReader[nextDate] === undefined) dateReader[nextDate] = [];
                        if (dateReader[nextDate].indexOf(data.readerId) == -1) {
                            dateReader[nextDate].push(data.readerId);
                        }
                    }
                });
            }
        }
        return dateReader;
    };
    Schedule.prototype.calculateTotal = function() {
        var o = this;
        var days = o.weekdays;
        o.weekdays.forEach(function(day, dindex) {
            var gtotal = 0;
            o.radioligists.forEach(function(r) {
                var cells = document.querySelectorAll("td[id^=" + `b_d${dindex}_${r.id}_` + "]");
                var totalCell = document.querySelector("td[id=" + `f0_d${dindex}_${r.id}` + "]");

                var total = 0;
                cells.forEach(function(cell) {
                    var val = parseInt(cell.innerText || "0");
                    total += val;
                });
                if (totalCell) {
                    totalCell.innerHTML = `<span><b>${total}</b></span>`;
                    totalCell.style.textAlign = "center";
                }
                gtotal += total;
            });
            if (o.showTotalColumn()) {
                var stat1TotalCell = document.querySelector("td[id=" + `f0_d${dindex}_stat1` + "]");
                if (stat1TotalCell) {
                    gtotal += parseInt(stat1TotalCell.innerText || "0");
                }
                var stat2TotalCell = document.querySelector("td[id=" + `f0_d${dindex}_stat2` + "]");
                if (stat2TotalCell) {
                    gtotal += parseInt(stat2TotalCell.innerText || "0");
                }
                //var casesTotalCell = document.querySelector("td[id=" + `f0_d${dindex}_cases` + "]");
                //if (casesTotalCell) {
                //    gtotal += parseInt(stat2TotalCell.innerText || "0");
                //}
                var grandTotalCell = document.querySelector("td[id=" + `f0_d${dindex}_total` + "]");
                grandTotalCell.innerHTML = `<span><b>${gtotal}</b></span>`;
                grandTotalCell.style.textAlign = "center";
            }
        });
    };
    Schedule.prototype.calculatePartialTotal = function(dindex, readerId) {
        var o = this;

        var gtotal = 0;
        var cells = document.querySelectorAll("td[id^=" + `b_d${dindex}_${readerId}_` + "]");
        var totalCell = document.querySelector("td[id=" + `f0_d${dindex}_${readerId}` + "]");

        var total = 0;
        cells.forEach(function(cell) {
            var val = parseInt(cell.innerText || "0");
            total += val;
        });
        if (totalCell) {
            totalCell.innerHTML = `<span><b>${total}</b></span>`;
            totalCell.style.textAlign = "center";
        }

        if (o.showTotalColumn()) {
            cells = document
                .querySelectorAll("td[id^=" + `f0_d${dindex}_` + "]");
            cells.forEach(function(cell) {
                if (!(/_total/.test(cell.id))) {
                    var val = parseInt(cell.innerText || "0");
                    gtotal += val;
                }
            });
            var grandTotalCell = document.querySelector("td[id=" + `f0_d${dindex}_total` + "]");
            grandTotalCell.innerHTML = `<span><b>${gtotal}</b></span>`;
            grandTotalCell.style.textAlign = "center";
        }

    };

    Schedule.prototype.markPrelims = function() {
        var o = this;
        var single = o.radioligists.length == 1;
        var cells=null;
        if(single){
                cells = document.querySelectorAll("td[id^=" + `b_d` + "]");
                for (var r = 0; r <= 23; r++) {
                    var readerIds = _.unique(
                                        Array.from(cells)
                                            .map(function(el) {return el;})
                                            .filter(i=> i.id.match(`^b_d\\d+_.{36}_${r}$`) && i.dataset.hash!==undefined)
                                                .map(function(i){ return i.id.match(`b_d\\d+_(.{36})_${r}`)[1];})
                                        );
                    var rights = _.max(o.radioligists
                                    .filter(function(i){ return readerIds.indexOf(i.id)>=0})
                                    .map(function(i) { return i.rights})
                                    );
                   
                    var cell = Array.from(cells)
                                    .map(function(el) {return el;})
                                    .find(function(el) { return el.id === `b_d0_prelim_${r}`});
                    var coverage = Array.from(cells)
                                    .map(function(el) {return el;})
                                    .find(function(el) { return el.id === `b_d0_coverage_${r}`});
                    var allMarked = Array.from(cells)
                                    .map(function(el) {return el;})
                                    .filter(i=> i.id.match(`^b_d\\d+_.{36}_${r}$`) && i.dataset.hash!==undefined)
                                    .map(function(i){ return i.id.match(`b_d\\d+_(.{36})_${r}`)[1];})
                                    .length;
                    var rad = o.radioligists.find(function(i){ return readerIds.indexOf(i.id)>=0 && i.rights>0; });
                    var counts = allMarked * ((rad && rad.thresholdPerHr) ||0);
                    //var statCell = Array.from(cells)
                    //                .map(function(el) {return el;})
                    //                .find(function(el) { return el.id === `b_d0_stat_${r}`});

                    //rad = o.radioligists.find(function(i){ return readerIds.indexOf(i.id)>=0; });
                    //var total = allMarked * ((rad && rad.thresholdPerHr) ||0);
                    //if(statCell) total += parseInt(statCell.innerText ||"0"); // add stats count 
  
                    if(cell){
                        switch(rights){ 
                            case 2:
                                cell.innerHTML = `<span style="height: 10px; width:10px; background: black; border-radius:50%;display:block;margin:6px;"></span>`;
                                break;
                            case 1:
                                cell.innerHTML = `<span style="height: 10px; width:10px; background: gray; border-radius:50%;display:block;margin:6px;"></span>`;
                                break;
                        }
                    }
                    if(coverage && counts>0){
                        coverage.innerHTML = `<span style="height: 96%; width:96%; background: black; border-radius:2px;display:block; color: white; padding-top:2px">${counts}</span>`;;
                    }
            }
            return;
        }

        o.weekdays.forEach(function(day, dindex) {
            cells = document.querySelectorAll("td[id^=" + `b_d${dindex}_` + "]");
            for (var r = 0; r <= 23; r++) {
                var readerIds = _.unique(
                                    Array.from(cells)
                                        .map(function(el) {return el;})
                                        .filter(i=> i.id.match(`^b_d${dindex}_.{36}_${r}$`) && i.dataset.hash!==undefined)
                                        .map(function(i){ return i.id.match(`b_d${dindex}_(.{36})_${r}`)[1];})
                                );
               var rights = _.max(o.radioligists
                                .filter(function(i){ return readerIds.indexOf(i.id)>=0})
                                .map(function(i) { return i.rights})
                                );
                var cell = Array.from(cells)
                                .map(function(el) {return el;})
                                .find(function(el) { return el.id === `b_d${dindex}_prelim_${r}`});

                var coverage = Array.from(cells)
                                .map(function(el) {return el;})
                                .find(function(el) { return el.id === `b_d${dindex}_coverage_${r}`});
                
                var counts = _.reduce(
                                o.radioligists.filter(function(i){ return readerIds.indexOf(i.id)>=0 && i.rights>0; }).map(function(i) {return i.thresholdPerHr}), 
                              function(a,b){return a+b;})||0;  
                var stat1Cell = Array.from(cells)
                                .map(function(el) {return el;})
                                .find(function(el) { return el.id === `b_d${dindex}_stat1_${r}`});
                var stat2Cell = Array.from(cells)
                    .map(function (el) { return el; })
                    .find(function (el) { return el.id === `b_d${dindex}_stat2_${r}` });
                 
                var totals = _.reduce(
                                o.radioligists.filter(function(i){ return readerIds.indexOf(i.id)>=0; }).map(function(i) {return i.thresholdPerHr}), 
                              function(a,b){return a+b;})||0;
                var caseTotals = totals;
                if (stat1Cell) totals += parseInt(stat1Cell.innerText || "0"); // add stats1 count
                if (stat2Cell) totals += parseInt(stat2Cell.innerText || "0"); // add stats2 count
                var totalCell = Array.from(cells)
                                .map(function(el) {return el;})
                                .find(function(el) { return el.id === `b_d${dindex}_total_${r}`});
                var casesCell = Array.from(cells)
                                .map(function(el) {return el;})
                                .find(function(el) { return el.id === `b_d${dindex}_cases_${r}`});
                if(cell){
                    switch(rights){ 
                        case 2:
                            cell.innerHTML = `<span style="height: 10px; width:10px; background: black; border-radius:50%;display:block;margin:6px;"></span>`;
                            break;
                        case 1:
                            cell.innerHTML = `<span style="height: 10px; width:10px; background: gray; border-radius:50%;display:block;margin:6px;"></span>`;
                            break;
                    }
                    //cell.className = `${hasPrelims?'yes':'no'}-prelim`;
                }
                if(coverage && counts>0){
                    coverage.innerHTML = `<span style="height: 96%; width:96%; background: black; border-radius:2px;display:block; color: white; padding-top:2px">${counts}</span>`;;
                }
                if (caseTotals){
                    casesCell.innerHTML = `<span><b>${caseTotals}</b></span>`;;
                    casesCell.style.textAlign = "center";
                }
                if(totalCell){
                    totalCell.innerHTML = `<span><b>${totals}</b></span>`;;
                    totalCell.style.textAlign = "center";
                }
            }
        });

    };


    Schedule.prototype.getDays = function() {

        var weekdays = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
        var wd = [];
        var d = new Date(this.fromDate);
        d.setHours(0, 0, 0, 0);
        var endDate = new Date(this.toDate);
        endDate.setHours(0, 0, 0, 0);
        do {
            wd.push({ day: weekdays[d.getDay()], date: d.getDate(), calendarDate: moment(d).format('YYYY-MM-DD') });
            d = new Date(+d + 24 * 3600 * 1000);
        } while (d <= endDate);
        return wd;
    }

    function getHours(hr) {
        var h = parseInt(hr.match(/(\d+):(\d+)/)[1]);
        var m = parseInt(hr.match(/(\d+):(\d+)/)[2]);
        if (m >= 30) h += 1;
        if (h > 24) h = 24;
        m = 0;
        var hh = h % 12;
        if (hh == 0) hh = 12;
        var ampm = "am";
        if (h > 11 && h < 24) ampm = "pm";
        return (hh > 9 ? hh.toString() : "0" + hh) + ampm;
    }

    function getTimeIndexes(timeslots, start, end) {
        var h1 = parseInt(start.match(/(\d+):(\d+)/)[1]);
        var m1 = parseInt(start.match(/(\d+):(\d+)/)[2]);
        if (m1 < 30) {
            m1 = 0;
        } else {
            h1 += 1;
            m1 = 0;
            if (h1 > 24) h1 = 24;
        }
        var hh1 = h1 % 12;
        if (hh1 == 0) hh1 = 12;
        var ampm1 = "am";
        if (h1 > 11 && h1 < 24) ampm1 = "pm";
        var from = (hh1 > 9 ? hh1.toString() : "0" + hh1) + ampm1;
        var startIndex = timeslots.indexOf(from);
        var h2 = parseInt(end.match(/(\d+):(\d+)/)[1]);
        var m2 = parseInt(end.match(/(\d+):(\d+)/)[2]);
        if (m2 < 30) {
            m2 = 0;
        } else {
            h2 += 1;
            m2 = 0;
        }
        var hh2 = h2 % 12;
        if (hh2 == 0) hh2 = 12;
        var ampm2 = "am";
        if (h2 > 11 && h2 < 24) ampm2 = "pm";
        var to = (hh2 > 9 ? hh2.toString() : "0" + hh2) + ampm2;
        var endIndex = timeslots.indexOf(to) - 1;
        if (endIndex < 0) endIndex = timeslots.length - 1;
        return [startIndex, endIndex];
    }

    function getContrast(hexcolor) {

        // If a leading # is provided, remove it
        if (hexcolor.slice(0, 1) === '#') {
            hexcolor = hexcolor.slice(1);
        }

        // If a three-character hexcode, make six-character
        if (hexcolor.length === 3) {
            hexcolor = hexcolor.split('').map(function(hex) {
                return hex + hex;
            }).join('');
        }

        // Convert to RGB value
        var r = parseInt(hexcolor.substr(0, 2), 16);
        var g = parseInt(hexcolor.substr(2, 2), 16);
        var b = parseInt(hexcolor.substr(4, 2), 16);

        // Get YIQ ratio
        var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;

        // Check contrast
        return (yiq >= 128) ? 'black' : 'white';

    }
    function shadeColor(color, percent) {

        var contrast=getContrast(color);
        var blend = 1;
        if(contrast=='white') blend=-1;
        var R = parseInt(color.substring(1,3),16);
        var G = parseInt(color.substring(3,5),16);
        var B = parseInt(color.substring(5,7),16);

        R = parseInt(R * (100 + percent*blend) / 100);
        G = parseInt(G * (100 + percent*blend) / 100);
        B = parseInt(B * (100 + percent*blend) / 100);

        R = (R<255)?R:255;  
        G = (G<255)?G:255;  
        B = (B<255)?B:255;  

        var RR = ((R.toString(16).length==1)?"0"+R.toString(16):R.toString(16));
        var GG = ((G.toString(16).length==1)?"0"+G.toString(16):G.toString(16));
        var BB = ((B.toString(16).length==1)?"0"+B.toString(16):B.toString(16));

        return "#"+RR+GG+BB;
    }

    Schedule.prototype.paint = function() {
        var o = this;
        var div = document.getElementById("schedulediv");
        div.style.display = "none";
        div.innerHTML = "";
        var table = document.createElement("table");

        table.setAttribute("id", "scheduletable");
        this.createHeader(table);
        this.createBody(table);
        this.createFooter(table);
        div.innerHTML = table.outerHTML;
        this.markAll();
        this.calculateTotal();
        this.markPrelims();
        div.style.display = "block";
        var tbl = document.getElementById("scheduletable");
        tbl.addEventListener("click", function(event) {
            if (event.target.tagName == "TD" && event.target.dataset.key) {
                var hash = event.target.dataset.hash;
                // find the lowest id because it can spilled to next/previous date depending on timezone
                var cells = Array.from(document.querySelectorAll(`td[data-hash="${hash}"]`))
                    .map(function(el) { return el })
                    .sort(function(a, b) {
                        var x = a.id.toLowerCase();
                        var y = b.id.toLowerCase();
                        if (x < y) { return -1; }
                        if (x > y) { return 1; }
                        return 0;
                    });
                var m = cells[0].getAttribute("id").match(/b_d(\d+)_(\w+-\w+-\w+-\w+-\w+)_(\d+)/);
                var timelineId = parseInt(m[3]);
                var readerId = m[2];
                var key = cells[0].dataset.key;
                if (o.onClickEvent) {
                    o.onClickEvent({
                        event: event,
                        data: {
                            key: key,
                            hash: hash,
                            index: (event.target.dataset.index == undefined ? -1 : parseInt(event.target.dataset.index)),
                            readerId: readerId,
                            timelineIndex: timelineId,
                            scope: o
                        }
                    });
                }
            }
        });

    };
    window.Schedule = new Schedule();
    }());

    $(document).ready($(function () {
        parent.window.scrollTo(0, 0);
        window.history.forward();
        parent.adjustFrameHeight();
        parent.HideLoad();
        parent.GsNavURL = ""; parent.GsRetStatus = "false"; parent.GsConfirmAction = "";
        parent.FetchMenuRecordCount()
 
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0;// For Chrome, Firefox, IE and Opera
    
    }))