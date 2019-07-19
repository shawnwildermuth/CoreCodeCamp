<template>
  <div>
    <h2>Users</h2>
      <div class="row">
        <table class="table table-bordered table-hover table-condensed">
            <tr>
              <th>Name</th>
              <th>UserName</th>
              <th>Email</th>
              <th>Email Confirmed</th>
              <th>Toggle</th>
              <th>Is Admin?</th>
              <th>Toggle</th>
            </tr>
          <tr v-for="user in users" :key="user.id">
            <td>{{user.name}}</td>
            <td>{{user.userName}}</td>
            <td>{{user.email}}</td>
            <td>{{user.isEmailConfirmed ? "Yes" : "No" }}</td>
            <td>
              <button class="btn btn-sm btn-primary" @click="onToggleConfirmation(user)" title="Toggle Confirmation"><i class="fa fa-check"></i> Confirm</button>
            </td>
            <td>{{ user.isAdmin ? "Yes" : "No" }}</td>
            <td>
              <button class="btn btn-sm btn-primary" @click="onToggleAdmin(user)" title="Toggle Admin"><i class="fa fa-check"></i> Admin</button>
            </td>
          </tr>
        </table>
      </div>
    </div>
</template>
<script>
// External JS Libraries
import { mapState } from "vuex";
export default {
  computed: mapState(["users"]),
  methods: {
    onToggleAdmin(user) {
      this.$store.dispatch("toggleUserAdmin", user);
    },

    onToggleConfirmation(user) {
      this.$store.dispatch("toggleUserConfirmation", user);
    }
  }
};
</script>