<template>
  <div>
    <h4>Talks</h4>
    <table class="table table-condensed table-bordered">
      <thead>
        <tr>
          <th class="col-md-5"><sortableHeader label="Title" :sortInfo="talkSort" columnName="title" sortAction="sortTalks"></sortableHeader></th>
          <th class="col-md-2"><sortableHeader label="Speaker" :sortInfo="talkSort" columnName="speaker" sortAction="sortTalks"></sortableHeader></th>
          <th class="col-md-2"><sortableHeader label="Category" :sortInfo="talkSort" columnName="category" sortAction="sortTalks"></sortableHeader></th>
          <th class="col-md-2"><sortableHeader label="Approve" :sortInfo="talkSort" columnName="approved" sortAction="sortTalks"></sortableHeader></th>
          <th class="col-md-1"></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="talk in talks">
          <tr :key="talk.id + 'a'">
            <td>{{ talk.title }}</td>
            <td>{{ talk.speaker.name }}</td>
            <td>{{ talk.category }}</td>
            <td>
              <button class="btn btn-sm btn-primary"
                      @click="toggleApprove(talk)">
                {{ talk.approved ? "Approved" : "Submitted" }} <small>(Click to Change)</small>
              </button>
            </td>
            <td>
              <button
                data-toggle="collapse"
                :data-target="'#talk' + talk.id"
                class="collapsed pull-right"
              >
                <i class="fa fa-expand show-collapsed"> More</i>
                <i class="fa fa-compress show-not-collapsed"> Less</i>
              </button>
            </td>
          </tr>
          <tr :key="talk.id + 'b'" :id="'talk' + talk.id" class="collapse">
            <td colspan="4">
              <talkBlock :talk="talk"></talkBlock>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import talkBlock from "./components/talkBlock";
import sortableHeader from "./components/sortableHeader";

export default {
  computed: mapState(["talks", "talkSort"]),
  components: { talkBlock, sortableHeader },
  methods: {
    ...mapActions(["sortTalks", "toggleApprove"])
  }
};
</script>

<style>
.collapse-row.collapsed + tr {
  display: none;
}

.collapsed > i.show-not-collapsed,
:not(.collapsed) > i.show-collapsed {
  display: none;
}
</style>
