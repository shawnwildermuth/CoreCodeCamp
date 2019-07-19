<template>
  <div>
    <h3>Sponsors</h3>
    <div v-cloak>
      <div>
        <router-link class="btn btn-success" :to="{ name: 'sponsorEditor', params: { type: 'new' } }">New Sponsor</router-link>
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
                <router-link class="btn btn-primary" :to="{ name: 'sponsorEditor', params: { type: 'edit',  id: sponsor.id } }">Edit</router-link>
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
    ...mapState(["sponsors"])
  },
  methods: {
    ...mapActions(["deleteSponsor", "toggleSponsorPaid"])
  }
};
</script>