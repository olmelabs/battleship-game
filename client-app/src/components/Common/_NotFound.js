import React from "react";
import i18n from "../../helpers/i18n";

class NotFound extends React.Component {
  constructor(props, context) {
    super(props, context);
  }

  render() {
    return (
      <div className="px-3 py-3 pt-md-5 pb-md-4 mx-auto text-center">
        <p className="lead">{i18n.t("common.notFound.message")}</p>
      </div>
    );
  }
}

export default NotFound;
