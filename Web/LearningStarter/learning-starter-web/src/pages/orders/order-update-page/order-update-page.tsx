import { Field, Form, Formik } from "formik";
import React, { useEffect, useState } from "react";
import { Button, Header, Input } from "semantic-ui-react";
import {
  ApiResponse,
  OrderGetDto,
  OrderUpdateDto,
} from "../../../constants/types";
import { useRouteMatch, useHistory } from "react-router-dom";
import axios from "axios";
import { Env } from "../../../config/env-vars";
import { baseUrl } from "../../../constants/constants";
import { routes } from "../../../routes/config";

export const OrderUpdatePage = () => {
  const [order, setOrder] = useState<OrderGetDto>();
  const { params } = useRouteMatch();
  const id = Number(params.id);

  const history = useHistory();

  useEffect(() => {
    const getOrder = async () => {
      const { data: response } = await axios.get<ApiResponse<OrderGetDto>>(
        `${baseUrl}/api/orders/${id}`
      );

      if (response.hasErrors) {
        alert(response.errors[0].message);
        return;
      }

      setOrder(response.data);
    };

    getOrder();
  }, []);

  const onSubmit = async (values: OrderUpdateDto) => {
    const { data: response } = await axios.put<ApiResponse<OrderGetDto>>(
      `${baseUrl}/api/orders/${id}`,
      values
    );

    if (response.hasErrors) {
      alert(response.errors[0].message);
      return;
    }

    alert("Succesfully updated thanks to Raid: Shadow Legends.");
    history.push(routes.order);
  };
  return (
    <>
      <Header>Edit Order {id}</Header>
      {order && (
        <Formik onSubmit={onSubmit} initialValues={order}>
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
      )}
    </>
  );
};
