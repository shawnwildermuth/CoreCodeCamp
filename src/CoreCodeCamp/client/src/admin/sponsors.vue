<template>
  <div>
    <h3>Sponsors</h3>
    <div v-cloak>
      <div>
        <button class="btn btn-success" @click="onNew()">New Sponsor</button>
      </div>
      <table class="table table-striped table-bordered table-hover">
        <thead>
          <tr>
            <th class="col-md-8">Name</th>
            <th class="col-md-1">Paid?</th>
            <th class="col-md-3"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="sponsor in sponsors" :key="sponsor.name">
            <td>{{sponsor.name}}</td>
            <td>{{sponsor.paid ? "Yes" : "No" }}</td>
            <td class="text-center">
              <div class="btn-group">
                <button
                  class="btn btn-primary"
                  @click="toggleSponsorPaid(sponsor)"
                >Toggle Paid</button>
                <button class="btn btn-primary" @click="onEdit(sponsor)">Edit</button>
                <button class="btn btn-danger" @click="deleteSponsor(sponsor)">Delete</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script>
// External JS Libraries
import { mapState, mapActions } from "vuex";

export default {
  computed: {
    validImage: function() {
      return this.currentSponsor &&
        this.currentSponsor.imageUrl &&
        this.currentSponsor.imageUrl.length > 0
        ? true
        : false;
    },
    ...mapState(["sponsors"])
  },
  methods: {
    onEdit(sponsor) {
      sponsor();
      // this.currentSponsor = sponsor;
      // this.$validator.validateAll();
    },
    // onNew() {
    //   this.currentSponsor = {};
    //   this.$validator.validateAll();
    // },
    ...mapActions(["deleteSponsor", "toggleSponsorPaid"])
  }
};
</script>