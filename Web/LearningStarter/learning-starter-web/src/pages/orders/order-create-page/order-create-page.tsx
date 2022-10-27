/** @format */

import { Field, Form, Formik } from "formik";
import React from "react";
import { Button, Header, Input } from "semantic-ui-react";
import { useHistory } from "react-router-dom";
import { routes } from "../../../routes/config";
import {
  ApiResponse,
  OrderCreateDto,
  OrderGetDto,
} from "../../../constants/types";
import axios from "axios";
import { baseUrl } from "../../../constants/constants";

export const OrderCreatePage = () => {
  const history = useHistory();

  const onSubmit = async (values: OrderCreateDto) => {
    const response = await axios.post<ApiResponse<OrderGetDto>>(
      `${baseUrl}/api/orders`,
      values
    );

    if (response.data.hasErrors) {
      alert("Something went incorrect.");
      return;
    }

    alert("Nothing went wrong :)");
    history.push(routes.order);
  };

  return (
    <>
      <Header>Create New Order</Header>
      <Formik onSubmit={onSubmit} initialValues={{} as OrderCreateDto}>
        <Form>
          <div>
            <div>
              <div className="field-label">
                <label htmlFor="paymentType">Payment Type</label>
              </div>
              <Field className="field" id="paymentType" name="paymentType">
                {({ field }) => <Input {...field} />}
              </Field>
            </div>
          </div>

          <div>
            <Button primary type="submit">
              Submit
            </Button>
            <Button
              secondary
              type="button"
              onClick={() => history.push(routes.order)}
            >
              Cancel
            </Button>
          </div>
        </Form>
      </Formik>
    </>
  );
};
