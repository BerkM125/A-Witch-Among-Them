using UnityEngine;

namespace CampaignObjects {

    public class Objective {
        public string name = "";
        public bool fulfilled = false;
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Objective(string name) {
            this.name = name;
            this.fulfilled = false;
        }
        public Objective(string name, Vector3 position) {
            this.name = name;
            this.fulfilled = false;
            this.position = position;
        }
    }

    public class Item {

    }

}