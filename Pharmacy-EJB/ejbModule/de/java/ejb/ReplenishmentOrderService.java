package de.java.ejb;

import java.util.Collection;
import java.util.Date;

import javax.ejb.Remote;

import de.java.domain.Drug;
import de.java.domain.OrderState;
import de.java.domain.Position;
import de.java.domain.ReplenishmentOrder;

@Remote
public interface ReplenishmentOrderService {

  Collection<ReplenishmentOrder> getAllReplenishmentOrders();

  Collection<ReplenishmentOrder> getReplenishmentOrdersInState(
      OrderState filterForState);

  ReplenishmentOrder getOrder(long id);

  ReplenishmentOrder getOrderWithPositions(long id);

  void proceedToNextState(long id);

  void cancel(long id);

  void updateExpectedDeliveryDate(long id, Date expectedDelivery);

  void updateActualDeliveryDate(long id, Date actualDelivery);

  Collection<Position> getPendingPositions(int pzn);

  void initiateReplenishment(Drug drug, long quantity);

  void removePosition(Position position);

}
