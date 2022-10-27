import axios from "axios";
import React, { useEffect, useState } from "react";
import { Button, Header, Icon, Segment, Tab, Table } from "semantic-ui-react";
import { ApiResponse, OrderGetDto } from "../../../constants/types";
import { useHistory } from "react-router-dom";
import { routes } from "../../../routes/config";

export const OrderListingPage = () => {
  const [orders, setOrders] = useState<OrderGetDto[]>();
  const [filter, setFilter] = useState(true);
  const history = useHistory();

  const goHome = () => {
    history.push(routes.home);
  };

  console.log({ orders });
  useEffect(() => {
    const fetchOrders = async () => {
      const { data: response } = await axios.get<ApiResponse<OrderGetDto[]>>(
        "api/orders"
      );

      if (response.hasErrors) {
        alert("Something went wrong.");
        return;
      }

      setOrders(response.data);
    };

    fetchOrders();
  }, []);

  return (
    <Segment>
      {orders && (
        <>
          <Header>Orders</Header>
          <Table striped celled>
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell width={1} />
                <Table.HeaderCell>Id</Table.HeaderCell>
                <Table.HeaderCell>Payment Type</Table.HeaderCell>
                <Table.HeaderCell>Date Purchased</Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {orders.map((order) => {
                return (
                  <Table.Row>
                    <Table.Cell>
                      <Icon
                        link
                        name="pencil"
                        onClick={() =>
                          history.push(
                            routes.orderUpdate.replace(":id", `${order.id}`)
                          )
                        }
                      />
                    </Table.Cell>
                    <Table.Cell>{order.id}</Table.Cell>
                    <Table.Cell>{order.paymentType}</Table.Cell>
                    <Table.Cell>{order.datePurchased}</Table.Cell>
                  </Table.Row>
                );
              })}
            </Table.Body>
          </Table>
        </>
      )}
      <Button onClick={goHome}>Go Home Nerd</Button>
      <Button onClick={() => setFilter(!filter)}>Toggle Filter</Button>
    </Segment>
  );
};
