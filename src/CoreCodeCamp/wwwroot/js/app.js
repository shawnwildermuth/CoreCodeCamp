var CodeCamp;
(function (CodeCamp) {
    var Common;
    (function (Common) {
        function createValidators() {
            var passwordValidation = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,25}$/;
            VeeValidate.Validator.extend('strongPassword', {
                getMessage: function (field) { return 'The ' + field + ' requires an uppercase, a lower case and a number.'; },
                validate: function (value) { return passwordValidation.test(value); }
            });
        }
        Common.createValidators = createValidators;
    })(Common = CodeCamp.Common || (CodeCamp.Common = {}));
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var Common;
    (function (Common) {
        function createFilters() {
            Vue.filter('formatDate', function (value) {
                var dt = null;
                if (typeof value === 'string') {
                    dt = moment(value);
                    if (!dt.isValid()) {
                        dt = moment(value, 'MM-DD-YYYY');
                    }
                }
                else if (value) {
                    dt = moment(String(value));
                }
                if (dt) {
                    return dt.format('MM-DD-YYYY');
                }
            });
            Vue.filter('formatTime', function (value) {
                if (value) {
                    return moment(String(value)).format('hh:mm a');
                }
            });
        }
        Common.createFilters = createFilters;
    })(Common = CodeCamp.Common || (CodeCamp.Common = {}));
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var Common;
    (function (Common) {
        function createDatePicker() {
            Vue.component('datepicker', {
                props: ['value'],
                template: '<input type="text" \
            class="v-datepicker" \
            ref="input" \
            v-bind:value="value | formatDate" \
            v-on:input="$emit(\'input\', $event.target.value)"/>',
                mounted: function () {
                    $(this.$el).datepicker({
                        dateFormat: "mm-dd-yy",
                        showOn: "button",
                        autoclose: true,
                        buttonImage: "/img/calendar.gif",
                        buttonImageOnly: true,
                        buttonText: "Select date",
                        onClose: this.onClose
                    });
                },
                methods: {
                    onClose: function (date) {
                        this.$emit('input', date);
                    }
                },
                watch: {
                    value: function (newVal) { $(this.el).datepicker('setDate', newVal); }
                }
            });
        }
        Common.createDatePicker = createDatePicker;
    })(Common = CodeCamp.Common || (CodeCamp.Common = {}));
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    CodeCamp.App = {
        setup: function () {
            Vue.use(VeeValidate);
            Vue.use(VueResource);
            CodeCamp.Common.createValidators();
            CodeCamp.Common.createFilters();
            CodeCamp.Common.createDatePicker();
            Vue.config.errorHandler = function (err, vm, info) {
                console.log(err);
            };
        },
        bootstrap: function (theView) {
            this.setup();
            new Vue(theView);
        },
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var Common;
    (function (Common) {
        var DataService = (function () {
            function DataService(http) {
                this.http = http;
            }
            DataService.prototype.baseUrl = function (moniker) {
                if (moniker === void 0) { moniker = this.moniker; }
                return '/' + moniker + "/api/";
            };
            Object.defineProperty(DataService.prototype, "moniker", {
                get: function () {
                    return window.location.pathname.split('/')[1];
                },
                enumerable: true,
                configurable: true
            });
            DataService.prototype.getAllEvents = function () {
                return this.http.get("/api/events/");
            };
            DataService.prototype.getEventInfo = function () {
                return this.http.get("/api/events/" + this.moniker);
            };
            DataService.prototype.saveEventInfo = function (eventInfo) {
                return this.http.put("/api/events/" + this.moniker, eventInfo);
            };
            DataService.prototype.saveEventLocation = function (location) {
                return this.http.put("/api/events/" + this.moniker + "/location", location);
            };
            DataService.prototype.addEventInfo = function (moniker) {
                return this.http.post("/api/events/" + moniker, { moniker: moniker });
            };
            DataService.prototype.getSponsors = function () {
                return this.http.get(this.baseUrl() + "sponsors");
            };
            DataService.prototype.saveSponsor = function (sponsor) {
                return this.http.post(this.baseUrl() + "sponsors", sponsor);
            };
            DataService.prototype.deleteSponsor = function (sponsor) {
                return this.http.delete(this.baseUrl() + "sponsors/" + sponsor.id);
            };
            DataService.prototype.togglePaid = function (sponsor) {
                return this.http.put(this.baseUrl() + "sponsors/" + sponsor.id + "/togglePaid/", null);
            };
            DataService.prototype.getMySpeaker = function () {
                return this.http.get(this.baseUrl() + "speakers/me");
            };
            DataService.prototype.saveSpeaker = function (speaker) {
                return this.http.post(this.baseUrl() + "speakers/me", speaker);
            };
            DataService.prototype.getTalks = function () {
                return this.http.get(this.baseUrl() + "talks/me");
            };
            DataService.prototype.getAllTalks = function () {
                return this.http.get(this.baseUrl() + "talks");
            };
            DataService.prototype.saveTalk = function (talk) {
                return this.http.post(this.baseUrl() + "speakers/me/talks", talk);
            };
            DataService.prototype.deleteTalk = function (id) {
                return this.http.delete(this.baseUrl() + "talks/" + id);
            };
            DataService.prototype.toggleApproved = function (talk) {
                return this.http.put(this.baseUrl() + "talks/" + talk.id + "/toggleApproved", talk);
            };
            DataService.prototype.updateTalkRoom = function (talk, value) {
                return this.http.put(this.baseUrl() + "talks/" + talk.id + "/room", { room: value });
            };
            DataService.prototype.updateTalkTime = function (talk, value) {
                return this.http.put(this.baseUrl() + "talks/" + talk.id + "/time", { time: value });
            };
            DataService.prototype.updateTalkTrack = function (talk, value) {
                return this.http.put(this.baseUrl() + "talks/" + talk.id + "/track", { track: value });
            };
            DataService.prototype.getUsers = function () {
                return this.http.get("/api/users");
            };
            DataService.prototype.toggleAdmin = function (user) {
                return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleAdmin", user);
            };
            DataService.prototype.toggleConfirmation = function (user) {
                return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleconfirmation", user);
            };
            DataService.prototype.getTimeSlots = function () {
                return this.http.get(this.baseUrl() + "timeSlots");
            };
            DataService.prototype.saveTimeSlot = function (timeSlot) {
                return this.http.post(this.baseUrl() + "timeSlots", { time: timeSlot });
            };
            DataService.prototype.deleteTimeSlot = function (timeSlot) {
                return this.http.delete(this.baseUrl() + "timeSlots/" + timeSlot.id);
            };
            DataService.prototype.getRooms = function () {
                return this.http.get(this.baseUrl() + "rooms");
            };
            DataService.prototype.saveRoom = function (room) {
                return this.http.post(this.baseUrl() + "rooms", { name: room });
            };
            DataService.prototype.deleteRoom = function (room) {
                return this.http.delete(this.baseUrl() + "rooms/" + room.id);
            };
            DataService.prototype.getTracks = function () {
                return this.http.get(this.baseUrl() + "tracks");
            };
            DataService.prototype.saveTrack = function (track) {
                return this.http.post(this.baseUrl() + "tracks", { name: track });
            };
            DataService.prototype.deleteTrack = function (track) {
                return this.http.delete(this.baseUrl() + "tracks/" + track.id);
            };
            DataService.prototype.formatError = function (err) {
                var msg = "";
                if (!err.body)
                    msg = "Unknown Error";
                else {
                    for (var key in err.body) {
                        var item = err.body[key];
                        msg += "<br/>" + key + ":" + item[0];
                    }
                }
                return msg;
            };
            return DataService;
        }());
        Common.DataService = DataService;
        Common.dataService = new DataService(Vue.http);
    })(Common = CodeCamp.Common || (CodeCamp.Common = {}));
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function adminEvents() {
        CodeCamp.App.bootstrap(CodeCamp.AdminEventsView);
    }
    CodeCamp.adminEvents = adminEvents;
    CodeCamp.AdminEventsView = {
        el: "#events-view",
        data: {
            campEvents: [],
            errorMessage: "",
            currentEvent: null,
            newEventMoniker: "",
            selectedModelMoniker: ""
        },
        methods: {
            onEventChanged: function (moniker) {
                this.currentEvent = _.find(this.campEvents, function (e) { return e.moniker === moniker; });
            },
            onAddEvent: function () {
                CodeCamp.Common.dataService.addEventInfo(this.newEventMoniker).then(function (result) {
                    this.campEvents.splice(0, 0, result.body);
                    this.currentEvent = result.body;
                    this.selectedModelMoniker = result.data.moniker;
                    this.newEventMoniker = "";
                }.bind(this), function () {
                    this.errorMessage = "Failed to save new event";
                }.bind(this));
            }
        },
        mounted: function () {
            CodeCamp.Common.dataService.getAllEvents()
                .then(function (result) {
                this.campEvents = result.data;
                this.currentEvent = _.first(this.campEvents);
                this.selectedModelMoniker = this.currentEvent.moniker;
            }.bind(this), function () {
                this.errorMessage = "Failed to get event data";
            }.bind(this));
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function adminSchedule() {
        CodeCamp.App.bootstrap(CodeCamp.adminScheduleView);
    }
    CodeCamp.adminSchedule = adminSchedule;
    CodeCamp.adminScheduleView = {
        el: "#schedule-view",
        data: {
            busy: true,
            talks: [],
            rooms: [],
            timeSlots: [],
            tracks: [],
            errorMessage: "",
            userMessage: "",
            sort: "",
            sortAsc: true,
            summary: {
                speakers: 0,
                approved: 0,
                talks: 0
            }
        },
        methods: {
            showError: function (err) {
                this.errorMessage = err;
            },
            setMsg: function (text) {
                var _this = this;
                this.userMessage = text;
                window.setTimeout(function () { return _this.userMessage = ""; }, 5000);
            },
            onTrackChanged: function (talk, $event) {
                var _this = this;
                this.busy = true;
                this.userMessage = "";
                this.errorMessage = "";
                var value = this.tracks[$event.target.selectedIndex];
                CodeCamp.Common.dataService.updateTalkTrack(talk, value.name)
                    .then(function () { return _this.setMsg("Saved..."); }, function () { return _this.showError("Failed to update talk"); })
                    .finally(function () { return _this.busy = false; });
            },
            onRoomChanged: function (talk, $event) {
                var _this = this;
                this.busy = true;
                this.userMessage = "";
                this.errorMessage = "";
                var value = this.rooms[$event.target.selectedIndex];
                CodeCamp.Common.dataService.updateTalkRoom(talk, value.name)
                    .then(function (result) {
                    _this.setMsg("Saved...");
                }, function (e) {
                    _this.showError("Failed to update talk");
                })
                    .finally(function () { return _this.busy = false; });
            },
            onTimeChanged: function (talk, $event) {
                var _this = this;
                this.busy = true;
                this.userMessage = "";
                this.errorMessage = "";
                var value = this.timeSlots[$event.target.selectedIndex];
                CodeCamp.Common.dataService.updateTalkTime(talk, value.time)
                    .then(function () { return _this.setMsg("Saved..."); }, function () { return _this.showError("Failed to update talk"); })
                    .finally(function () { return _this.busy = false; });
            },
            onSort: function (sort) {
                var _this = this;
                if (sort == this.sort) {
                    if (this.sortAsc)
                        this.sortAsc = false;
                    else {
                        this.sort = "title";
                        this.sortAsc = true;
                    }
                }
                else {
                    this.sort = sort;
                    this.sortAsc = true;
                }
                this.talks = this.talks.sort(function (a, b) {
                    if (a[_this.sort] == b[_this.sort])
                        return 0;
                    if (a[_this.sort] < b[_this.sort])
                        return _this.sortAsc ? -1 : 1;
                    else
                        return _this.sortAsc ? 1 : -1;
                });
            },
            updateSummary: function () {
                this.summary.approved = this.talks.filter(function (t) { return t.approved; }).length;
                this.summary.talks = this.talks.length;
                this.summary.speakers = this.talks.map(function (t) { return t.speaker.name; }).filter(function (x, i, s) { return s.indexOf(x) === i; }).length;
            },
            onDelete: function (talk) {
                var _this = this;
                this.busy = true;
                CodeCamp.Common.dataService.deleteTalk(talk.id)
                    .then(function () {
                    _this.talks.splice(_this.talks.indexOf(talk), 1);
                    _this.updateSummary();
                }, function (e) { return _this.showError("Failed to delete talk"); })
                    .finally(function () { return _this.busy = false; });
            },
            onToggleApproved: function (talk) {
                var _this = this;
                this.busy = true;
                CodeCamp.Common.dataService.toggleApproved(talk)
                    .then(function (result) {
                    talk.approved = !talk.approved;
                    _this.updateSummary();
                }, function (e) { return _this.showError("Failed to toggle approved flag"); })
                    .finally(function () { return _this.busy = false; });
            }
        },
        mounted: function () {
            Vue.Promise.all([
                CodeCamp.Common.dataService.getAllTalks(),
                CodeCamp.Common.dataService.getRooms(),
                CodeCamp.Common.dataService.getTimeSlots(),
                CodeCamp.Common.dataService.getTracks()
            ])
                .then(function (result) {
                this.talks = result[0].data;
                this.rooms = result[1].data;
                this.timeSlots = result[2].data;
                this.tracks = result[3].data;
                this.updateSummary();
            }.bind(this), function () {
                this.errorMessage = "Failed to load data";
            }.bind(this))
                .finally(function () {
                this.busy = false;
            }.bind(this));
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function adminSponsors() {
        CodeCamp.App.bootstrap(CodeCamp.adminSponsorsView);
    }
    CodeCamp.adminSponsors = adminSponsors;
    CodeCamp.adminSponsorsView = {
        el: "#sponsors-view",
        data: {
            busy: true,
            sponsors: [],
            currentSponsor: null,
            errorMessage: "",
            userMessage: "",
            imageError: ""
        },
        computed: {
            validImage: function () {
                return (this.currentSponsor && this.currentSponsor.imageUrl && this.currentSponsor.imageUrl.length > 0) ? true : false;
            },
            isPristine: function () {
                return;
            }
        },
        methods: {
            onTogglePaid: function (sponsor) {
                var _this = this;
                this.busy = true;
                CodeCamp.Common.dataService.togglePaid(sponsor)
                    .then(function () { return sponsor.paid = !sponsor.paid; }, function () { return _this.errorMessage = "Could not toggle paid"; })
                    .finally(function () { return _this.busy = false; });
            },
            onEdit: function (sponsor) {
                this.currentSponsor = sponsor;
                this.$validator.validateAll();
            },
            onDelete: function (sponsor) {
                var _this = this;
                this.busy = true;
                CodeCamp.Common.dataService.deleteSponsor(sponsor)
                    .then(function () { return _this.sponsors.splice(_this.sponsors.indexOf(sponsor), 1); }, function () { return _this.errorMessage = "Could not delete sponsor"; })
                    .finally(function () { return _this.busy = false; });
            },
            onNew: function () {
                this.currentSponsor = {};
                this.$validator.validateAll();
            },
            onSave: function () {
                var _this = this;
                var old = this.sponsors.indexOf(this.currentSponsor);
                if (old > -1)
                    this.sponsors.splice(this.sponsors.indexOf(this.currentSponsor), 1);
                this.busy = true;
                CodeCamp.Common.dataService.saveSponsor(this.currentSponsor)
                    .then(function (res) {
                    _this.sponsors.push(res.data);
                    _this.currentSponsor = null;
                }, function () { return _this.errorMessage = "Could not save sponsor"; })
                    .finally(function () { return _this.busy = false; });
            },
            onCancel: function () {
                this.currentSponsor = null;
            },
            onImagePicked: function (filePicker) {
                var _this = this;
                this.busy = true;
                var file = jQuery("#thePicker")[0].files[0];
                CodeCamp.Common.imageUploadService.uploadSponsor(file)
                    .then(function (imageUrl) {
                    Vue.set(_this.currentSponsor, "imageUrl", imageUrl);
                }, function (e) { return _this.errorMessage = "Failed to upload Image"; })
                    .finally(function () { return _this.busy = false; });
            },
            onShowPicker: function () {
                jQuery("#thePicker").click();
            }
        },
        mounted: function () {
            var _this = this;
            CodeCamp.Common.dataService.getSponsors()
                .then(function (result) {
                this.sponsors = result.data;
                this.currentSponsor = null;
            }.bind(this), function () { return _this.errorMessage = "Failed to load data"; })
                .finally(function () { return _this.busy = false; });
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var Common;
    (function (Common) {
        var ImageUploadService = (function () {
            function ImageUploadService() {
            }
            ImageUploadService.prototype.uploadSpeaker = function (img) {
                return this.uploadImage(img, "speakers");
            };
            ImageUploadService.prototype.uploadSponsor = function (img, moniker) {
                if (moniker === void 0) { moniker = CodeCamp.Common.dataService.moniker; }
                return this.uploadImage(img, "sponsors", moniker);
            };
            ImageUploadService.prototype.uploadImage = function (file, imageType, moniker) {
                if (moniker === void 0) { moniker = CodeCamp.Common.dataService.moniker; }
                return new Vue.Promise(function (resolve, reject) {
                    var xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === 4) {
                            if (xhr.status >= 200 && xhr.status <= 299) {
                                var location_1 = xhr.getResponseHeader("location");
                                if (!location_1)
                                    location_1 = JSON.parse(xhr.responseText).location;
                                resolve(location_1);
                            }
                            else {
                                reject(xhr.response);
                            }
                        }
                    };
                    xhr.open('POST', "/" + moniker + "/api/images/" + imageType, true);
                    var formData = new FormData();
                    formData.append("file", file, file.name);
                    xhr.send(formData);
                });
            };
            return ImageUploadService;
        }());
        Common.ImageUploadService = ImageUploadService;
        Common.imageUploadService = new ImageUploadService();
    })(Common = CodeCamp.Common || (CodeCamp.Common = {}));
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function callForSpeakers() {
        Vue.use(VueRouter);
        CodeCamp.App.bootstrap({ router: CodeCamp.callForSpeakersRouter.router, el: "#call-for-speaker-view" });
    }
    CodeCamp.callForSpeakers = callForSpeakers;
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    CodeCamp.SpeakerEditorView = {
        template: "#speaker-editor",
        data: function () {
            return {
                busy: true,
                errorMessage: "",
                infoMessage: "",
                speaker: {},
                imageError: "",
                filePicker: {}
            };
        },
        methods: {
            onSaveSpeaker: function () {
                var _this = this;
                this.$validator.validateAll().then(function (result) {
                    if (result) {
                        _this.busy = true;
                        _this.errorMessage = "";
                        _this.infoMessage = "";
                        CodeCamp.speakerData.saveSpeaker(_this.speaker)
                            .then(function () {
                            CodeCamp.callForSpeakersRouter.router.push({ name: "info" });
                            this.infoMessage = "Saved...";
                        }.bind(_this), function (err) {
                            this.errorMessage = "Failed to save speaker. Please check your input fields for errors: " + CodeCamp.Common.dataService.formatError(err);
                        }.bind(_this))
                            .finally(function () {
                            this.busy = false;
                        }.bind(_this));
                    }
                });
            },
            onFilePicker: function () {
                this.$refs.filePicker.click();
            },
            onImagePicked: function () {
                var _this = this;
                this.isBusy = true;
                this.$imgService.uploadSpeaker(this.$refs.filePicker.files[0])
                    .then(function (imageUrl) {
                    _this.speaker.imageUrl = imageUrl;
                    _this.$forceUpdate();
                }, function (e) {
                    _this.imageError = e;
                })
                    .finally(function () { return _this.isBusy = false; });
            },
            validImage: function () {
                return this.speaker && this.speaker.imageUrl && this.speaker.imageUrl.length > 0;
            }
        },
        mounted: function () {
            this.$imgService = new CodeCamp.Common.ImageUploadService();
            CodeCamp.speakerData.getSpeaker()
                .then(function (skr) {
                if (skr)
                    this.speaker = skr;
                this.busy = false;
            }.bind(this), function () {
                this.errorMessage = "Failed to load speaker";
                this.busy = false;
            }.bind(this));
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var SpeakerService = (function () {
        function SpeakerService() {
            this._speaker = null;
        }
        SpeakerService.prototype.getSpeaker = function () {
            var _this = this;
            return new Vue.Promise(function (resolve, reject) {
                if (this._speaker == null) {
                    Vue.Promise.all([
                        CodeCamp.Common.dataService.getMySpeaker(),
                        CodeCamp.Common.dataService.getTalks()
                    ])
                        .then(function (result) {
                        _this._speaker = result[0].data;
                        _this._speaker.talks = result[1].data;
                        resolve(_this._speaker);
                    }, function () {
                        reject();
                    });
                }
                else {
                    resolve(_this._speaker);
                }
            });
        };
        SpeakerService.prototype.saveSpeaker = function (speaker) {
            this._speaker = speaker;
            return CodeCamp.Common.dataService.saveSpeaker(speaker);
        };
        SpeakerService.prototype.deleteTalk = function (talk) {
            var _this = this;
            return new Vue.Promise(function (resolve, reject) {
                CodeCamp.Common.dataService.deleteTalk(talk.id)
                    .then(function () {
                    _this._speaker.talks.splice(_this._speaker.talks.indexOf(talk), 1);
                    resolve();
                }, function () { return reject(); });
            });
        };
        SpeakerService.prototype.saveTalk = function (talk) {
            var _this = this;
            return new Vue.Promise(function (resolve, reject) {
                CodeCamp.Common.dataService.saveTalk(talk)
                    .then(function (result) {
                    var resultTalk = result.body;
                    var talk = _.find(_this._speaker.talks, function (t) { return t.id == resultTalk.id; });
                    if (!talk) {
                        _this._speaker.talks.push(resultTalk);
                    }
                    resolve();
                }, function (err) {
                    reject(err.data);
                });
            });
        };
        return SpeakerService;
    }());
    CodeCamp.SpeakerService = SpeakerService;
    CodeCamp.speakerData = new SpeakerService();
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    CodeCamp.SpeakerInfoView = {
        template: "#speaker-info",
        data: function () {
            return {
                busy: true,
                errorMessage: "",
                speaker: null
            };
        },
        methods: {
            onDeleteTalk: function (talk) {
                var _this_1 = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.speakerData.deleteTalk(talk)
                    .then(function () { }, function () { return _this_1.errorMessage = "Failed to delete talk"; })
                    .finally(function () { return _this_1.busy = false; });
            }
        },
        mounted: function () {
            if (!this.speaker) {
                var _this_2 = this;
                CodeCamp.speakerData.getSpeaker()
                    .then(function (skr) {
                    if (!skr || skr.id == 0) {
                        CodeCamp.callForSpeakersRouter.router.push({ name: "editor" });
                    }
                    else {
                        _this_2.speaker = skr;
                    }
                }, function () { return _this_2.errorMessage = "Failed to load speaker"; });
            }
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var Common;
    (function (Common) {
        Common.helpers = {
            isPristine: function (fields) {
                return Object.keys(fields).every(function (field) {
                    return fields[field] && fields[field].pristine;
                });
            }
        };
    })(Common = CodeCamp.Common || (CodeCamp.Common = {}));
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    CodeCamp.SpeakerTalkEditorView = {
        template: "#speaker-talk-editor",
        data: function () {
            return {
                busy: true,
                errorMessage: "",
                speaker: {},
                talk: {}
            };
        },
        methods: {
            onSave: function () {
                var _this = this;
                this.$validator.validateAll().then(function (result) {
                    if (result) {
                        _this.busy = true;
                        _this.errorMessage = "";
                        CodeCamp.speakerData.saveTalk(_this.talk).then(function (result) {
                            CodeCamp.callForSpeakersRouter.router.push({ name: "info" });
                        }, function (err) {
                            this.errorMessage = "Failed to save speaker: " + err.bodyText;
                        }).then(function () { return _this.busy = false; });
                    }
                });
            }
        },
        computed: {
            isPristine: function () {
                return CodeCamp.Common.helpers.isPristine(this.fields);
            }
        },
        mounted: function () {
            CodeCamp.speakerData.getSpeaker()
                .then(function (skr) {
                if (skr)
                    this.speaker = skr;
                var theId = this.$route.params.id;
                if (theId != 'new') {
                    this.talk = _.find(this.speaker.talks, function (t) { return t.id == theId; });
                }
                this.busy = false;
            }.bind(this), function () {
                this.errorMessage = "Failed to load speaker";
                this.busy = false;
            }.bind(this));
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var routes = [
        { path: "/", redirect: { name: "info" } },
        { path: "/info", name: "info", component: CodeCamp.SpeakerInfoView },
        { path: "/edit", name: "editor", component: CodeCamp.SpeakerEditorView },
        { path: "/talks/:id", name: "talkEditor", component: CodeCamp.SpeakerTalkEditorView, props: true },
        { path: "*", redirect: { name: "info" } }
    ];
    CodeCamp.callForSpeakersRouter = {
        router: new VueRouter({
            routes: routes
        })
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    var Talk = (function () {
        function Talk() {
            this.id = 0;
            this.audience = "Developers";
            this.level = "Beginner";
            this.category = "General Discussion";
        }
        return Talk;
    }());
    CodeCamp.Talk = Talk;
    ;
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function eventInfo() {
        CodeCamp.App.bootstrap(CodeCamp.EventInfoView);
    }
    CodeCamp.eventInfo = eventInfo;
    CodeCamp.EventInfoView = {
        el: "#event-info-view",
        data: {
            busy: true,
            theEvent: {},
            errorMessage: "",
            eventMessage: "",
            locationMessage: "",
            rooms: [],
            timeSlots: [],
            tracks: [],
            newRoom: "",
            newTrack: "",
            newTimeSlot: ""
        },
        methods: {
            onSaveEvent: function () {
                var _this = this;
                this.$validator.validateAll("theEvent").then(function (result) {
                    if (result) {
                        _this.busy = true;
                        _this.errorMessage = "";
                        _this.eventMessage = "Please Wait...";
                        CodeCamp.Common.dataService.saveEventInfo(_this.theEvent).then(function () {
                            this.eventMessage = "Saved...";
                        }.bind(_this), function () {
                            this.eventMessage = "Failed to save changes...";
                        }.bind(_this)).finally(function () { return _this.busy = false; }).bind(_this);
                    }
                }, function () { this.eventMessage = "Please fix any validation errors..."; });
                return false;
            },
            onSaveLocation: function () {
                var _this = this;
                this.$validator.validateAll("vLocation").then(function (result) {
                    if (result) {
                        _this.busy = true;
                        _this.errorMessage = "";
                        _this.locationMessage = "";
                        CodeCamp.Common.dataService.saveEventLocation(_this.theEvent.location).then(function () {
                            this.locationMessage = "Saved...";
                        }, function () {
                            this.locationMessage = "Failed to save changes...";
                        }).finally(function () { return _this.busy = false; });
                    }
                });
                return false;
            },
            onSaveTrack: function () {
                var _this = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.Common.dataService.saveTrack(this.newTrack)
                    .then(function (result) {
                    _this.tracks.push(result.data);
                    _this.newTrack = "";
                }, function () { return _this.errorMessage = "Failed to save track"; })
                    .finally(function () { return _this.busy = false; });
            },
            onSaveRoom: function () {
                var _this = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.Common.dataService.saveRoom(this.newRoom)
                    .then(function (result) {
                    _this.rooms.push(result.data);
                    _this.newRoom = "";
                }, function (e) {
                    _this.errorMessage = "Failed to save room";
                }).finally(function () { return _this.busy = false; });
            },
            onSaveTimeSlot: function () {
                var _this = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.Common.dataService.saveTimeSlot(this.newTimeSlot)
                    .then(function (result) {
                    _this.timeSlots.push(result.data);
                    _this.newTimeSlot = "";
                }, function (e) {
                    _this.errorMessage = "Failed to save timeslot";
                }).finally(function () { return _this.busy = false; });
            },
            onDeleteTrack: function (track) {
                var _this = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.Common.dataService.deleteTrack(track)
                    .then(function () { return _this.tracks.splice(_this.tracks.indexOf(track), 1); }, function () { return _this.errorMessage = "Failed to delete track"; })
                    .finally(function () { return _this.busy = false; });
            },
            onDeleteRoom: function (room) {
                var _this = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.Common.dataService.deleteRoom(room)
                    .then(function (result) {
                    _this.rooms.splice(_this.rooms.indexOf(room), 1);
                }, function (e) { return _this.errorMessage = "Failed to delete room"; })
                    .finally(function () { return _this.busy = false; });
            },
            onDeleteTimeSlot: function (timeSlot) {
                var _this = this;
                this.busy = true;
                this.errorMessage = "";
                CodeCamp.Common.dataService.deleteTimeSlot(timeSlot)
                    .then(function (result) {
                    _this.timeSlots.splice(_this.timeSlots.indexOf(timeSlot), 1);
                }, function (e) { return _this.errorMessage = "Failed to delete timeslot"; })
                    .finally(function () { return _this.busy = false; });
            }
        },
        computed: {},
        mounted: function () {
            Vue.Promise.all([
                CodeCamp.Common.dataService.getEventInfo(),
                CodeCamp.Common.dataService.getTimeSlots(),
                CodeCamp.Common.dataService.getRooms(),
                CodeCamp.Common.dataService.getTracks()
            ])
                .then(function (result) {
                this.theEvent = result[0].data;
                this.timeSlots = result[1].data;
                this.rooms = result[2].data;
                this.tracks = result[3].data;
            }.bind(this), function () {
                this.errorMessage = "Failed to load data";
            }.bind(this))
                .finally(function () {
                this.busy = false;
                this.$validator.validateAll('vEvent').then(function () { }).catch(function () { });
                this.$validator.validateAll('vLocation').then(function () { }).catch(function () { });
            }.bind(this));
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    CodeCamp.JoinView = {
        el: "#join-view",
        data: {
            user: {
                name: "",
                email: "",
                password: "",
                confirmPassword: ""
            },
            errorMessage: ""
        },
        computed: {
            isPristine: function () {
                return CodeCamp.Common.helpers.isPristine(this.fields);
            }
        },
        methods: {
            onSubmit: function () {
                var me = this;
                this.$validator.validateAll().then(function (success) {
                    if (!success) {
                        me.errorMessage = "Please fix validation Issues";
                        return false;
                    }
                });
            },
            created: function () {
                this.$set(this, 'errors', this.$validator.errorBag);
            }
        }
    };
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function join() {
        CodeCamp.App.bootstrap(CodeCamp.JoinView);
    }
    CodeCamp.join = join;
})(CodeCamp || (CodeCamp = {}));
var CodeCamp;
(function (CodeCamp) {
    function users() {
        CodeCamp.App.bootstrap(CodeCamp.UsersView);
    }
    CodeCamp.users = users;
    CodeCamp.UsersView = {
        el: "#users-view",
        data: {
            users: [],
            errorMessage: "",
            busy: true
        },
        methods: {
            onToggleAdmin: function (user) {
                var _this = this;
                this.busy = true;
                this.$dataService.toggleAdmin(user).then(function (result) {
                    user.isAdmin = result.data;
                }, function () {
                    this.errorMessage = "Failed to toggle admin";
                }).finally(function () { return _this.busy = false; });
            },
            onToggleConfirmation: function (user) {
                var _this = this;
                this.busy = true;
                this.$dataService.toggleConfirmation(user).then(function (result) {
                    user.isEmailConfirmed = result.data;
                }, function () {
                    this.errorMessage = "Failed to toggle confirmation";
                }).finally(function () { return _this.busy = false; });
            }
        },
        mounted: function () {
            var _this = this;
            this.$dataService = new CodeCamp.Common.DataService(this.$http);
            this.$dataService.getUsers().then(function (result) {
                this.users = result.data;
            }, function () {
                this.errorMessage = "Failed to get user data";
            }).finally(function () { return _this.busy = false; });
        }
    };
})(CodeCamp || (CodeCamp = {}));
//# sourceMappingURL=/js/app.js.map