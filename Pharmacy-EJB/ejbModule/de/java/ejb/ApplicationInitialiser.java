package de.java.ejb;

import javax.annotation.PostConstruct;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

import de.java.domain.Drug;

@Singleton
@Startup
public class ApplicationInitialiser {

  @PersistenceContext
  private EntityManager em;

  @PostConstruct
  public void initialise() {
    if (noDrugsPersisted()) {
      populateAppWithSampleDrugs();
    }
  }

  private boolean noDrugsPersisted() {
    return em.createQuery("FROM Drug").getResultList().size() == 0;
  }

  private void populateAppWithSampleDrugs() {
    em.persist(new Drug(8456716, "ASPIRIN PLUS C ORANGE 10St"));
    em.persist(new Drug(1715965, "ASPIRIN PLUS C ORANGE 20St"));
  }
}