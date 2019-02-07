import React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";

class ProfilePage extends React.Component {
  constructor(props, context) {
    super(props, context);

    const user = JSON.parse(localStorage.getItem("user"));
    this.state = {
      email: user.email,
      firstName: user.firstName,
      lastName: user.lastName
    };
  }

  render() {
    return (
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-4 col-xs-12">
          <form>
            <div className="form-group">
              <label htmlFor="txtEmail">Email address</label>
              <input
                type="email"
                className="form-control"
                id="txtEmail"
                placeholder="Email"
                value={this.state.email}
                readOnly
              />
            </div>
            <div className="form-group">
              <label htmlFor="txtFirstName">First Name</label>
              <input
                type="text"
                className="form-control"
                id="txtFirstName"
                placeholder="First Name"
                value={this.state.firstName}
                readOnly
              />
            </div>
            <div className="form-group">
              <label htmlFor="txtLastName">Last Name</label>
              <input
                type="text"
                className="form-control"
                id="txtLastName"
                placeholder="Last Name"
                value={this.state.lastName}
                readOnly
              />
            </div>
            {/* <button type="submit" className="btn btn-primary">Submit</button> */}
          </form>
        </div>
      </div>
    );
  }
}

const mapStateToProps = (state, ownProps) => ({});

export default connect(mapStateToProps)(ProfilePage);
