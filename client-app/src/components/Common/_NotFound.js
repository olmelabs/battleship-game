import React from "react";

class NotFound extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    return (
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">Nothing is here.</p>
      </div>
    );
  }
}

export default NotFound;
