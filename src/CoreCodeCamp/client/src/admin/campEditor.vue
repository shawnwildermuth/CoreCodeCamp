<template>
  <div class="row">
    <form novalidate data-vv-scope="vEvent" @submit.prevent="onValidateForm('vEvent')">
      <div class="col-md-6">
        <div class="form-group" :class="{ error: errors.has('vEvent.name') }">
          <label class="text-danger">Name *</label>
          <input
            v-model="theEvent.name"
            class="form-control"
            placeholder="Your Name"
            name="name"
            autofocus
            v-validate="'required|min:5'"
          />
          <div class="help-block">{{ errors.first('vEvent.name') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.first('vEvent.description') }">
          <label class="text-danger">Description *</label>
          <textarea
            v-model="theEvent.description"
            cols="40"
            rows="6"
            class="form-control"
            name="description"
            v-validate="'required|min:100'"
          ></textarea>
          <div class="help-block">{{ errors.first('vEvent.description') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.moniker') }">
          <label class="text-danger">Moniker *</label>
          <input
            v-model="theEvent.moniker"
            class="form-control"
            placeholder="The prefix for the website."
            name="moniker"
            v-validate="{ rules: { required: true, regex: /^[a-zA-Z0-9-_]+$/ } }"
          />
          <div class="help-block">{{ errors.first('vEvent.moniker') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.eventDate') }">
          <label class="text-danger">Event Date *</label>
          <div>
            <datepicker v-model.lazy="theEvent.eventDate" class="form-control" name="eventDate"></datepicker>
          </div>
          <div class="help-block">{{ errors.first('vEvent.eventDate') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.eventLength') }">
          <label class="text-danger">Event Length *</label>
          <input
            v-model.number="theEvent.eventLength"
            class="form-control"
            name="eventLength"
            v-validate="'min_value:1'"
          />
          <div class="help-block">{{ errors.first('vEvent.eventLength') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.callForSpeakersOpened') }">
          <label class="text-danger">Call For Speaker Opening Date *</label>
          <div>
            <datepicker
              v-model.lazy="theEvent.callForSpeakersOpened"
              class="form-control"
              name="callForSpeakersOpened"
            ></datepicker>
          </div>
          <div class="help-block">{{ errors.first('vEvent.callForSpeakersOpened') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.callForSpeakersClosed') }">
          <label class="text-danger">Call For Speaker Closing Date *</label>
          <div>
            <datepicker
              v-model.lazy="theEvent.callForSpeakersClosed"
              class="form-control"
              name="callForSpeakersClosed"
            ></datepicker>
          </div>
          <div class="help-block">{{ errors.first('vEvent.callForSpeakersClosed') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.contactEmail') }">
          <label class="text-danger">Contact Email *</label>
          <input
            v-model="theEvent.contactEmail"
            class="form-control"
            placeholder="Contact Email"
            name="contactEmail"
            v-validate="'email'"
          />
          <div class="help-block">{{ errors.first('vEvent.contactEmail') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.facebookLink') }">
          <label>Facebook Link</label>
          <input
            v-model="theEvent.facebookLink"
            class="form-control"
            placeholder="Link to a Faceboook Page"
            name="facebookLink"
            v-validate="'url'"
          />
          <div class="help-block">{{ errors.first('vEvent.facebookLink') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.twitterLink') }">
          <label>Twitter Link</label>
          <input
            v-model="theEvent.twitterLink"
            class="form-control"
            placeholder="Link to Twitter"
            name="twitterLink"
            v-validate="'url'"
          />
          <div class="help-block">{{ errors.first('vEvent.twitterLink') }}</div>
        </div>
        <div class="form-group" :class="{ error: errors.has('vEvent.instagramLink') }">
          <label>Instagram Link</label>
          <input
            v-model="theEvent.instagramLink"
            class="form-control"
            placeholder="Link to Instagram"
            name="instagramLink"
            v-validate="'url'"
          />
          <div class="help-block">{{ errors.first('vEvent.instagramLink') }}</div>
        </div>
        <div class="form-group">
          <label>EventBrite ID</label>
          <input
            v-model="theEvent.registrationLink"
            class="form-control"
            placeholder="Link to Meetup or EventBrite"
            name="registrationLink"
          />
        </div>
        <div class="form-group">
          <input
            type="button"
            class="btn btn-success btn-lg"
            value="Save"
            v-bind:disabled="errors.any('vLocation')"
            @click="onSave()"
          />
          <router-link to="/" class="btn btn-info btn-lg">Cancel</router-link>
          <p class="text-success" v-if="campMessage">{{ campMessage }}</p>
        </div>
      </div>
      <div class="col-md-6">
        <div>
          <div class="form-group">
            <label>Facility Name</label>
            <input
              v-model="theEvent.location.facility"
              class="form-control"
              placeholder="Name of the facility"
              name="facility"
            />
          </div>
          <div class="form-group">
            <label>Address</label>
            <input
              v-model="theEvent.location.address1"
              class="form-control"
              placeholder="Address"
              name="address1"
            />
          </div>
          <div class="form-group">
            <label>Address (2nd Line)</label>
            <input
              v-model="theEvent.location.address2"
              class="form-control"
              placeholder
              name="address2"
            />
          </div>
          <div class="form-group">
            <label>City</label>
            <input
              v-model="theEvent.location.city"
              class="form-control"
              placeholder="City"
              name="city"
            />
          </div>
          <div class="form-group">
            <label>State</label>
            <input
              v-model="theEvent.location.stateProvince"
              class="form-control"
              placeholder="State"
              name="stateProvince"
            />
          </div>
          <div class="form-group">
            <label>Postal Code</label>
            <input
              v-model="theEvent.location.postalCode"
              class="form-control"
              placeholder="e.g. 12345"
              name="postalCode"
            />
          </div>
          <div class="form-group">
            <label>Country</label>
            <input
              v-model="theEvent.location.country"
              class="form-control"
              placeholder="USA"
              name="country"
            />
          </div>
          <div class="form-group" :class="{ error: errors.has('vLocation.link') }">
            <label>Facility Web URL</label>
            <input
              v-model="theEvent.location.link"
              class="form-control"
              placeholder="A web address"
              name="link"
              v-validate="'url'"
            />
            <div class="help-block">{{ errors.first('vLocation.link') }}</div>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";
import _ from "lodash";

export default {
  data: () => {
    return {
      theEvent: {
        location: {}
      },
      campMessage: ""
    };
  },
  props: ["camp"],
  mounted: function() {
    if (this.$route.params.type == "edit") {
      // TODO Make copy, can't change vuex state
      // Also, use ID so we can repeat it
      this.theEvent = _.cloneDeep(this.$store.state.currentCamp);
    }

    if (!this.theEvent.location) this.theEvent.location = {};
  },
  computed: mapState(["currentCamp"]),
  methods: {
    ...mapActions(["updateCamp", "saveCamp"]),
    onSave: function() {
      this.$validator.validateAll("theEvent").then(
        result => {
          if (result) {
            if (this.$route.params.type == "edit") {
              this.updateCamp(this.theEvent).then(() => {
                this.campMessage = "Saved...";
                this.$router.push("/");
              });
            } else {
              this.saveCamp(this.theEvent).then(() => {
                this.campMessage = "Saved...";
                this.$router.push("/");
              });
            }
          }
        },
        function() {
          this.campMessage = "Please fix any validation errors...";
        }
      );
      return false;
    }
  }
};
</script>

<style>
</style>
