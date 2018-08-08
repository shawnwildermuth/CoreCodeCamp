declare namespace CodeCamp.Common {
    function createValidators(): void;
}
declare namespace CodeCamp.Common {
    function createFilters(): void;
}
declare namespace CodeCamp.Common {
    function createDatePicker(): void;
}
declare namespace CodeCamp {
    let App: {
        setup: () => void;
        bootstrap: (theView: any) => void;
    };
}
declare namespace CodeCamp.Common {
    class DataService {
        private http;
        constructor(http: any);
        private baseUrl;
        readonly moniker: string;
        getAllEvents(): any;
        getEventInfo(): any;
        saveEventInfo(eventInfo: any): any;
        saveEventLocation(location: any): any;
        addEventInfo(moniker: any): any;
        getSponsors(): any;
        saveSponsor(sponsor: any): any;
        deleteSponsor(sponsor: any): any;
        togglePaid(sponsor: any): any;
        getMySpeaker(): any;
        saveSpeaker(speaker: any): any;
        getTalks(): any;
        getAllTalks(): any;
        saveTalk(talk: any): any;
        deleteTalk(id: Number): any;
        toggleApproved(talk: any): any;
        updateTalkRoom(talk: any, value: any): any;
        updateTalkTime(talk: any, value: any): any;
        updateTalkTrack(talk: any, value: any): any;
        getUsers(): any;
        toggleAdmin(user: any): any;
        toggleConfirmation(user: any): any;
        getTimeSlots(): any;
        saveTimeSlot(timeSlot: any): any;
        deleteTimeSlot(timeSlot: any): any;
        getRooms(): any;
        saveRoom(room: any): any;
        deleteRoom(room: any): any;
        getTracks(): any;
        saveTrack(track: any): any;
        deleteTrack(track: any): any;
    }
    let dataService: DataService;
}
declare namespace CodeCamp {
    function adminEvents(): void;
    let AdminEventsView: {
        el: string;
        data: {
            campEvents: any[];
            errorMessage: string;
            currentEvent: any;
            newEventMoniker: string;
            selectedModelMoniker: string;
        };
        methods: {
            onEventChanged(moniker: any): void;
            onAddEvent(): void;
        };
        mounted(): void;
    };
}
declare namespace CodeCamp {
    function adminSchedule(): void;
    let adminScheduleView: {
        el: string;
        data: {
            busy: boolean;
            talks: any[];
            rooms: any[];
            timeSlots: any[];
            tracks: any[];
            errorMessage: string;
            userMessage: string;
            sort: string;
            sortAsc: boolean;
            summary: {
                speakers: number;
                approved: number;
                talks: number;
            };
        };
        methods: {
            showError(err: any): void;
            setMsg(text: string): void;
            onTrackChanged(talk: any, $event: any): void;
            onRoomChanged(talk: any, $event: any): void;
            onTimeChanged(talk: any, $event: any): void;
            onSort(sort: any): void;
            updateSummary(): void;
            onDelete(talk: any): void;
            onToggleApproved(talk: any): void;
        };
        mounted(): void;
    };
}
declare namespace CodeCamp {
    function adminSponsors(): void;
    let adminSponsorsView: {
        el: string;
        data: {
            busy: boolean;
            sponsors: any[];
            currentSponsor: any;
            errorMessage: string;
            userMessage: string;
            imageError: string;
        };
        computed: {
            validImage: () => boolean;
            isPristine: () => void;
        };
        methods: {
            onTogglePaid(sponsor: any): void;
            onEdit(sponsor: any): void;
            onDelete(sponsor: any): void;
            onNew(): void;
            onSave(): void;
            onCancel(): void;
            onImagePicked(filePicker: any): void;
            onShowPicker(): void;
        };
        mounted(): void;
    };
}
declare namespace CodeCamp.Common {
    class ImageUploadService {
        uploadSpeaker(img: File): any;
        uploadSponsor(img: File, moniker?: string): any;
        private uploadImage;
    }
    let imageUploadService: ImageUploadService;
}
declare namespace CodeCamp {
    function callForSpeakers(): void;
}
declare namespace CodeCamp {
    let SpeakerEditorView: {
        template: string;
        data(): {
            busy: boolean;
            errorMessage: string;
            infoMessage: string;
            speaker: {};
            imageError: string;
            filePicker: {};
        };
        methods: {
            onSaveSpeaker(): void;
            onFilePicker(): void;
            onImagePicked(): void;
            validImage(): boolean;
        };
        mounted(): void;
    };
}
declare namespace CodeCamp {
    class SpeakerService {
        _speaker: any;
        getSpeaker(): any;
        saveSpeaker(speaker: any): any;
        deleteTalk(talk: any): any;
        saveTalk(talk: any): any;
    }
    var speakerData: SpeakerService;
}
declare namespace CodeCamp {
    let SpeakerInfoView: {
        template: string;
        data(): {
            busy: boolean;
            errorMessage: string;
            speaker: any;
        };
        methods: {
            onDeleteTalk(talk: any): void;
        };
        mounted(): void;
    };
}
declare namespace CodeCamp.Common {
    let helpers: {
        isPristine(fields: any): boolean;
    };
}
declare namespace CodeCamp {
    let SpeakerTalkEditorView: {
        template: string;
        data(): {
            busy: boolean;
            errorMessage: string;
            speaker: {};
            talk: {};
        };
        methods: {
            onSave(): void;
        };
        computed: {
            isPristine: () => boolean;
        };
        mounted(): void;
    };
}
declare namespace CodeCamp {
    let callForSpeakersRouter: {
        router: any;
    };
}
declare namespace CodeCamp {
    class Talk {
        id: number;
        title: string;
        abstract: string;
        prerequisites: string;
        audience: string;
        level: string;
        category: string;
    }
}
declare namespace CodeCamp {
    function eventInfo(): void;
    let EventInfoView: {
        el: string;
        data: {
            busy: boolean;
            theEvent: {};
            errorMessage: string;
            eventMessage: string;
            locationMessage: string;
            rooms: any[];
            timeSlots: any[];
            tracks: any[];
            newRoom: string;
            newTrack: string;
            newTimeSlot: string;
        };
        methods: {
            onSaveEvent(): boolean;
            onSaveLocation(): boolean;
            onSaveTrack(): void;
            onSaveRoom(): void;
            onSaveTimeSlot(): void;
            onDeleteTrack(track: any): void;
            onDeleteRoom(room: any): void;
            onDeleteTimeSlot(timeSlot: any): void;
        };
        computed: {};
        mounted(): void;
    };
}
declare namespace CodeCamp {
    let JoinView: {
        el: string;
        data: {
            user: {
                name: string;
                email: string;
                password: string;
                confirmPassword: string;
            };
            errorMessage: string;
        };
        computed: {
            isPristine: () => boolean;
        };
        methods: {
            onSubmit(): void;
            created(): void;
        };
    };
}
declare namespace CodeCamp {
    function join(): void;
}
declare namespace CodeCamp {
    function users(): void;
    let UsersView: {
        el: string;
        data: {
            users: any[];
            errorMessage: string;
            busy: boolean;
        };
        methods: {
            onToggleAdmin(user: any): void;
            onToggleConfirmation(user: any): void;
        };
        mounted(): void;
    };
}
