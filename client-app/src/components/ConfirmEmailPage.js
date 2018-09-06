import React from 'react';
import { connect } from 'react-redux';

class ConfirmEmailPage extends React.Component {
  constructor(props, context){
    super(props, context);
  }

  render() {
    return(
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">Confirm email page</p>
      </div>
      );
  }

}

export default connect()(ConfirmEmailPage);
